using HotelBooking.Application.CQRS.User.DTOs;

namespace HotelBooking.Application.CQRS.User.Commands.ChangePassword
{
    public class ChangePasswordCommand : ICommand<Result>
    {
        public int UserId { get; set; }
        public ChangePasswordDto Password { get; set; }
    }
}