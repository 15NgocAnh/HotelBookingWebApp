namespace HotelBooking.Domain.DTOs.Room
{
    /// <summary>
    /// Represents the conditions for booking a room, including check-in and check-out dates,
    /// room type, and the number of adults and children.
    /// </summary>
    public class RoomCondition
    {
        /// <summary>
        /// Gets or sets the check-in date for the room booking.
        /// </summary>
        public DateTime CheckInDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Gets or sets the check-out date for the room booking.
        /// </summary>
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

        /// <summary>
        /// Gets or sets the type of room being booked.
        /// </summary>
        public int RoomType { get; set; }

        /// <summary>
        /// Gets or sets the number of adults for the room booking.
        /// </summary>
        public int AdultsCnt { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of children for the room booking.
        /// </summary>
        public int ChildrenCnt { get; set; } = 0;
    }
}
