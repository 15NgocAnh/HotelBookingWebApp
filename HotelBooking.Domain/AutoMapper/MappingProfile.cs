using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Domain.DTOs.Post;
using HotelBooking.Domain.DTOs.Role;
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
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(x => x.phone_number));
            CreateMap<UserModel, UserDTO>()
                 .ForMember(dto => dto.roles, opt => opt.MapFrom(x => x.user_roles.Select(y => y.role).ToList()))
                 .ForMember(dto => dto.avatar, opt => opt.MapFrom(x => x.Avatar));
            CreateMap<JwtDTO, JWTModel>().ForMember(dest => dest.token_hash_value, opt => opt.MapFrom(src => _pwdHasher.md5(src.Token)));
            CreateMap<UserInfoDTO, UserModel>();
            CreateMap<UserModel, UserInfoDTO>().ReverseMap();
            CreateMap<UserModel, UserDetailsDTO>()
                .ForMember(dest => dest.dob, opt => opt.MapFrom(src => src.DOB))
                .ForMember(dest => dest.posts, opt => opt.MapFrom(src => src.posts))
                .ForMember(dest => dest.roles, opt => opt.MapFrom(src => src.user_roles.Select(y => y.role).ToList()))
                .ForMember(dto => dto.avatar, opt => opt.MapFrom(x => x.Avatar));
            CreateMap<UserModel, CredentialDTO>()
                .ForMember(dto => dto.roles, opt => opt.MapFrom(x => x.user_roles.Select(y => y.role).ToList()))
                .ForMember(dto => dto.avatar, opt => opt.MapFrom(x => x.Avatar));
            CreateMap<RoleModel, RolesDTO>().ReverseMap();
            CreateMap<PostModel, PostDTO>().ForMember(dto => dto.file_name, opt => opt.MapFrom(x => x.file.name))
                                           .ForMember(dto => dto.file_type, opt => opt.MapFrom(x => x.file.type))
                                           .ForMember(dto => dto.file_url, opt => opt.MapFrom(x => x.file.url))
                                           .ForMember(dto => dto.author, opt => opt.MapFrom(x => x.user.LastName)).ReverseMap();
            CreateMap<PostModel, PostValidatorDTO>()
                                            .ForMember(dto => dto.file_url, opt => opt.MapFrom(x => x.file.url))
                                            .ForMember(dto => dto.author, opt => opt.MapFrom(x => x.user.LastName)).ReverseMap();
            CreateMap<PostModel, PostDetailsDTO>()
                                            .ForMember(dto => dto.file_url, opt => opt.MapFrom(x => x.file.url))
                                            .ForMember(dto => dto.avatar_author, opt => opt.MapFrom(x => x.user.Avatar))
                                            .ForMember(dto => dto.author, opt => opt.MapFrom(x => x.user.LastName))
                                            .ReverseMap();
        }
    }
}
