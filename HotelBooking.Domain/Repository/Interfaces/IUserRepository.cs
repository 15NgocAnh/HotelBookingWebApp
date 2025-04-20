using HotelBooking.Domain.DTOs.User;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
        Task<UserModel> GetUserByEmail(string email);
        Task<bool> UpdateAvatar(string? userId, string avatar);
        Task<bool> ChangePasswordAsync(string newPassword, UserModel user);
        Task<UserModel> UpdateAsync(UserInfoDTO userDTO, UserModel userModel);
        Task<UserModel> FindUserPostAsync(int userId);
        Task<UserModel> GetDetailsUserAsync(int userId);
        Task<UserModel> GetUserNotIsAdminAsync(int id);
        int GetRoleByUserId(int id);
    }
}
