using HotelBooking.Domain.DTOs.User;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
        Task<UserModel> GetUserByEmail(string email);
        Task<bool> UpdateAvatar(string? userId, string avatar);
        Task<bool> ChangePasswordAsync(string newPassword, UserModel user);
        Task<UserModel> UpdateAsync(UserInfoDTO userDTO, UserModel userModel);
        Task<UserModel> GetUserNotIsAdminAsync(int id);
        Task<UserModel> GetDetailsUserAsync(int userId);
        int GetRoleByUserId(int id);
    }
}
