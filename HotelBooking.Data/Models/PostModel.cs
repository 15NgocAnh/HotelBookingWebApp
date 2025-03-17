using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace HotelBooking.Data.Models
{
    public class PostModel : BaseModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public string caption { get; set; }
        public bool is_actived { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public int user_id { get; set; }
        [JsonIgnore]
        [ForeignKey("user_id")]
        public UserModel user { get; set; }
        public int? job_id { get; set; } = null;
        public int file_id { get; set; }
        [JsonIgnore]
        [ForeignKey("file_id")]
        public FileModel file { get; set; }

    }
}
