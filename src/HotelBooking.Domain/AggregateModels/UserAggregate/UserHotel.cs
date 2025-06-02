using HotelBooking.Domain.AggregateModels.HotelAggregate;

namespace HotelBooking.Domain.AggregateModels.UserAggregate
{
    public class UserHotel : BaseEntity, IAggregateRoot
    {
        public int UserId { get; private set; }
        public int HotelId { get; private set; }

        public virtual User User { get; private set; } = null!;
        public virtual Hotel Hotel { get; private set; } = null!;

        private UserHotel() { } // For EF

        public UserHotel(User user, Hotel hotel)
        {
            UserId = user.Id;
            HotelId = hotel.Id;
            User = user;
            Hotel = hotel;
        }
    }
}
