using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Data.Models
{
    public class BookingModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingID { get; set; }

        [ForeignKey("Hotel")]
        public string HotelCode { get; set; }

        [ForeignKey("Guest")]
        public int GuestID { get; set; }

        [ForeignKey("Room")]
        public string RoomNo { get; set; }

        public DateTime BookingDateTime { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string EstArrivalTime { get; set; }
        public string EstDepartureTime { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public string SpecialReq { get; set; }
        public string BookingStatus { get; set; } // Pending, Confirmed, Cancelled, Checked-in, Checked-out

        public virtual HotelModel Hotel { get; set; }
        public virtual UserModel Guest { get; set; }
        public virtual RoomModel Room { get; set; }
    }

}
