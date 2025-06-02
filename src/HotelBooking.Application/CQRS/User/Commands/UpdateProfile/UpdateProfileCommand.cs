using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.User.DTOs;

namespace HotelBooking.Application.CQRS.User.Commands.UpdateProfile
{
    public class UpdateProfileCommand : ICommand<Result>
    {
        public int UserId { get; set; }
        public UpdateProfileDto Profile { get; set; }
    }
}