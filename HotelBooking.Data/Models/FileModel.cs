using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{

    public class FileModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression("\\w+.(jpg|mp4){1}")]
        public string name { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public string type { get; set; }
        public string url { get; set; }
        public virtual ICollection<PostModel> posts { get; set; }


    }
}
