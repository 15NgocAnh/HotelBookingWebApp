using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Domain.Encryption;
using HotelBooking.Domain.Entities;
namespace HotelBooking.Domain.AutoMapper
{
    public class MappingProfile : Profile
    {
        private readonly IPasswordHasher _pwdHasher;
        public MappingProfile(IPasswordHasher pwdHasher)
        {
            _pwdHasher = pwdHasher;
            CreateMap<UserRegisterDTO, UserModel>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(scr => _pwdHasher.Hash(scr.password)))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(x => x.first_name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(x => x.last_name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(x => x.phone_number));
            CreateMap<UserModel, UserDTO>()
                 .ForMember(dto => dto.roles, opt => opt.MapFrom(x => x.UserRoles.Select(y => y.Role).ToList()))
                 .ForMember(dto => dto.first_name, opt => opt.MapFrom(x => x.FirstName))
                 .ForMember(dto => dto.last_name, opt => opt.MapFrom(x => x.LastName))
                 .ForMember(dto => dto.avatar, opt => opt.MapFrom(x => x.ProfileImage));
            CreateMap<JwtDTO, JWTModel>()
                .ForMember(dest => dest.TokenHashValue, opt => opt.MapFrom(src => _pwdHasher.md5(src.Token)))
                .ForMember(dest => dest.ExpiredDate, opt => opt.MapFrom(src => src.ExpiredDate));
            CreateMap<UserInfoDTO, UserModel>();
            CreateMap<UserModel, UserInfoDTO>().ReverseMap();
            CreateMap<UserModel, UserDetailsDTO>()
                .ForMember(dest => dest.dob, opt => opt.MapFrom(src => src.DOB))
                .ForMember(dest => dest.roles, opt => opt.MapFrom(src => src.UserRoles.Select(y => y.Role).ToList()))
                .ForMember(dto => dto.avatar, opt => opt.MapFrom(x => x.ProfileImage));
            CreateMap<UserModel, CredentialDTO>()
                .ForMember(dto => dto.roles, opt => opt.MapFrom(x => x.UserRoles.Select(y => y.Role).ToList()))
                .ForMember(dto => dto.avatar, opt => opt.MapFrom(x => x.ProfileImage));
            CreateMap<RoleModel, RoleDto>().ReverseMap();
            CreateMap<RoleModel, CreateRoleDto>().ReverseMap();
            CreateMap<RoleModel, UpdateRoleDto>().ReverseMap();
            CreateMap<PermissionModel, PermissionDto>().ReverseMap();

            CreateMap<Room, RoomDTO>()
                .ForMember(dto => dto.RoomTypeName, opt => opt.MapFrom(x => x.RoomType.Name))
                .ForMember(dto => dto.NumberOfAdults, opt => opt.MapFrom(x => x.RoomType.NumberOfAdults))
                .ForMember(dto => dto.NumberOfChildrent, opt => opt.MapFrom(x => x.RoomType.NumberOfChildrent))
                .ReverseMap();
            CreateMap<Room, CreateRoomDTO>().ReverseMap();
            CreateMap<Room, UpdateRoomDTO>().ReverseMap();
            CreateMap<UpdateRoomDTO, RoomDTO>().ReverseMap();
            CreateMap<CreateRoomDTO, RoomDTO>().ReverseMap();

            CreateMap<RoomType, RoomTypeDTO>().ReverseMap();
            CreateMap<RoomType, CreateRoomTypeDTO>().ReverseMap();
            CreateMap<RoomType, UpdateRoomTypeDTO>().ReverseMap();
            CreateMap<CreateRoomTypeDTO, RoomTypeDTO>().ReverseMap();
            CreateMap<UpdateRoomTypeDTO, RoomTypeDTO>().ReverseMap();

            CreateMap<Floor, FloorDTO>().ReverseMap();
            CreateMap<Floor, CreateFloorDTO>().ReverseMap();
            CreateMap<Floor, UpdateFloorDTO>().ReverseMap();
            CreateMap<FloorDTO, CreateFloorDTO>().ReverseMap();
            CreateMap<FloorDTO, UpdateFloorDTO>().ReverseMap();

            CreateMap<BookingModel, BookingDTO>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Room))
                .ForMember(dest => dest.GuestID, opt => opt.MapFrom(src => src.Guest));
            CreateMap<BookingDTO, BookingModel>();

            CreateMap<BranchDetailsDTO, BranchModel>().ReverseMap();
            CreateMap<BranchDTO, BranchModel>().ReverseMap();
            CreateMap<BranchCreateDTO, BranchModel>().ReverseMap();
        }
    }
}
