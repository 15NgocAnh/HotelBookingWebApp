using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    public class JWTModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string token_hash_value { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime expired_date { get; set; }
        public string? fcm_token { get; set; }
        public int user_id { get; set; }
        [JsonIgnore]
        [ForeignKey("user_id")]
        public UserModel user { get; set; }
    }
}
