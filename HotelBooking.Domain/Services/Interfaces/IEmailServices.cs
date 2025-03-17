using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Services.Interfaces
{
    public interface IEmailServices
    {
        Task sendActivationEmail(UserModel user);
        Task sendForgotPassword(UserModel user);
    }
}
