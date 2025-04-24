using HotelBooking.Domain.DTOs.File;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IUserServices
    {
        Task<ServiceResponse<UserDTO>> RegisterAsync(UserRegisterDTO user);
        Task<ServiceResponse<UserInfoDTO>> updateUserInfo(UserInfoDTO updateUser, string? id);
        Task<ServiceResponse<UserDetailsDTO>> GetUserInfoAsync(int id);
        Task<ServiceResponse<UserDetailsDTO?>> GetUserByIdAsync(int user_id, int currentUser_id);
        Task<ServiceResponse<object>> changePassword(UPasswordDTO passwordDTO, string? id);
        Task updateAvatar(FileDTO fileDTO, string? id);
    }
}
