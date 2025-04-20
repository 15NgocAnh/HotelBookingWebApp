using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.DTOs.Post;
using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Domain.Encryption;
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
                .ForMember(dest => dest.posts, opt => opt.MapFrom(src => src.Posts))
                .ForMember(dest => dest.roles, opt => opt.MapFrom(src => src.UserRoles.Select(y => y.Role).ToList()))
                .ForMember(dto => dto.avatar, opt => opt.MapFrom(x => x.ProfileImage));
            CreateMap<UserModel, CredentialDTO>()
                .ForMember(dto => dto.roles, opt => opt.MapFrom(x => x.UserRoles.Select(y => y.Role).ToList()))
                .ForMember(dto => dto.avatar, opt => opt.MapFrom(x => x.ProfileImage));
            CreateMap<RoleModel, RolesDTO>().ReverseMap();
            CreateMap<PostModel, PostDTO>().ForMember(dto => dto.file_name, opt => opt.MapFrom(x => x.File.Name))
                                           .ForMember(dto => dto.file_type, opt => opt.MapFrom(x => x.File.Type))
                                           .ForMember(dto => dto.file_url, opt => opt.MapFrom(x => x.File.Url))
                                           .ForMember(dto => dto.author, opt => opt.MapFrom(x => x.User.LastName)).ReverseMap();
            CreateMap<PostModel, PostValidatorDTO>()
                                            .ForMember(dto => dto.file_url, opt => opt.MapFrom(x => x.File.Url))
                                            .ForMember(dto => dto.author, opt => opt.MapFrom(x => x.User.LastName)).ReverseMap();
            CreateMap<PostModel, PostDetailsDTO>()
                                            .ForMember(dto => dto.file_url, opt => opt.MapFrom(x => x.File.Url))
                                            .ForMember(dto => dto.avatar_author, opt => opt.MapFrom(x => x.User.ProfileImage))
                                            .ForMember(dto => dto.author, opt => opt.MapFrom(x => x.User.LastName))
                                            .ReverseMap();
            CreateMap<RoomModel, RoomDTO>().ForMember(dto => dto.RoomType, opt => opt.MapFrom(x => x.RoomType.Name))
                                            .ReverseMap();
            CreateMap<RoomModel, RoomDetailsDTO>()
                .ForMember(dto => dto.RoomType, opt => opt.MapFrom(src => src.RoomType.Id))
                .ReverseMap()
                .ForPath(src => src.RoomType.Id, opt => opt.MapFrom(dto => dto.RoomType));

            // Add direct mapping between RoomDTO and RoomDetailsDTO
            CreateMap<RoomDTO, RoomDetailsDTO>()
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => int.Parse(src.RoomType)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<RoomDetailsDTO, RoomDTO>()
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<BookingModel, BookingDTO>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Room))
                .ForMember(dest => dest.GuestID, opt => opt.MapFrom(src => src.Guest));
            CreateMap<BookingDTO, BookingModel>();
        }
    }
}
