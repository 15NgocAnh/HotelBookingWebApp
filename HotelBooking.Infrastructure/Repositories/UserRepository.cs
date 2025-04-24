using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.DTOs.User;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Constant;

namespace HotelBooking.Domain.Repository
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        public UserRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            var user = await _context.Users
                .Where(x => x.Email == email)
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> UpdateAvatar(string? userId, string avatar)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));
                if (user != null)
                {
                    user.ProfileImage = avatar;
                }
                else
                {
                    return false;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(string newPassword, UserModel user)
        {
            try
            {
                user.PasswordHash = newPassword;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UserModel> UpdateAsync(UserInfoDTO userDTO, UserModel userModel)
        {
            var user = _mapper.Map(userDTO, userModel);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel> GetUserNotIsAdminAsync(int id)
        {
            var role = await _context.Roles.Where(c => c.Name == CJConstant.ADMIN).FirstOrDefaultAsync();

            var user = await _context.UserRoles
                .Where(x => x.UserId == id && x.RoleId != role.Id)
                .Select(u => u.User)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<UserModel> GetDetailsUserAsync(int userId)
        {
            return await _context.Users
                .Include(c => c.UserRoles)
                    .ThenInclude(r => r.Role)
                .FirstAsync(c => c.Id == userId);
        }

        public int GetRoleByUserId(int id)
        {
            return _context.UserRoles
                .Where(u => u.UserId == id)
                .Select(u => u.RoleId)
                .FirstOrDefault();
        }
    }
}
