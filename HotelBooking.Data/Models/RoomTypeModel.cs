using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Data.Models
{
    public class RoomTypeModel : BaseModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RoomType { get; set; }
        public decimal RoomPrice { get; set; }
        public decimal DefaultRoomPrice { get; set; }
        public string RoomImg { get; set; }
        public string RoomDesc { get; set; }
        public ICollection<RoomModel> Rooms { get; set; }
    }
}
