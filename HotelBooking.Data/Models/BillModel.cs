using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Data.Models
{
    public class BillModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceNo { get; set; }

        [ForeignKey("Booking")]
        public int BookingID { get; set; }

        [ForeignKey("Guest")]
        public int GuestID { get; set; }

        public decimal RoomCharge { get; set; }
        public decimal RoomService { get; set; }
        public decimal RestaurantCharges { get; set; }
        public decimal BarCharges { get; set; }
        public decimal MiscCharges { get; set; }
        public decimal IfLateCheckout { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; } // Cash, Credit Card, Online Payment

        public virtual BookingModel Booking { get; set; }
        public virtual UserModel Guest { get; set; }
    }

}
