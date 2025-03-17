using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Data.Models
{
    public class HotelModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string HotelCode { get; set; }

        public string HotelName { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int NumRooms { get; set; }
        public string PhoneNo { get; set; }
        public int StarRating { get; set; }

        public ICollection<RoomModel> Rooms { get; set; }
        public ICollection<BookingModel> Bookings { get; set; }
    }

}
