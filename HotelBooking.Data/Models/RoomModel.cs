using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Data.Models
{
    public class RoomModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RoomNo { get; set; }

        [ForeignKey("Hotel")]
        public string HotelCode { get; set; }

        [ForeignKey("RoomType")]
        public string RoomType { get; set; }

        public string Status { get; set; } // Available, Booked, Maintenance

        public virtual HotelModel Hotel { get; set; }
        public virtual RoomTypeModel RoomTypeNavigation { get; set; }
    }

}
