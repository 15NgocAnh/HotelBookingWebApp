using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IEmailServices
    {
        Task sendActivationEmail(UserModel user);
        Task sendForgotPassword(UserModel user);
    }
}
