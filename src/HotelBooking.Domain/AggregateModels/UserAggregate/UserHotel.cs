using HotelBooking.Domain.AggregateModels.HotelAggregate;
using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.UserAggregate
{
    /// <summary>
    /// Represents the relationship between a user and a hotel in the system.
    /// This is an aggregate root entity that manages the many-to-many relationship between users and hotels.
    /// </summary>
    public class UserHotel : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// Gets the ID of the associated user.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Gets the ID of the associated hotel.
        /// </summary>
        public int HotelId { get; private set; }

        /// <summary>
        /// Gets the associated user.
        /// </summary>
        public virtual User User { get; private set; } = null!;

        /// <summary>
        /// Gets the associated hotel.
        /// </summary>
        public virtual Hotel Hotel { get; private set; } = null!;

        /// <summary>
        /// Initializes a new instance of the UserHotel class.
        /// Required for Entity Framework Core.
        /// </summary>
        private UserHotel() { }

        /// <summary>
        /// Initializes a new instance of the UserHotel class with specified user and hotel.
        /// </summary>
        /// <param name="user">The user to associate with the hotel.</param>
        /// <param name="hotel">The hotel to associate with the user.</param>
        public UserHotel(User user, Hotel hotel)
        {
            UserId = user.Id;
            HotelId = hotel.Id;
            User = user;
            Hotel = hotel;
        }
    }
}
