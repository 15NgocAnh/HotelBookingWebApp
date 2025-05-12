using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.Common.Security;
public interface IAppUserManager
{
    Task<IAppUser?> FindByUserNameAsync(string userName);
    Task<IAppUser?> FindByUserIdAsync(string userId);
    Task<Result<IAppUser>> CreateAsync(IAppUser user, string password);
    Task<Result<IAppUser>> UpdateAsync(IAppUser user);
    Task<Result> DeleteAsync(IAppUser user);
}
