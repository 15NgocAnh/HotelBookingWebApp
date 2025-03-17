using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    public class UserRoleModel : BaseModel
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int user_id { get; set; }
        public int role_id { get; set; }
        [JsonIgnore]
        [ForeignKey("user_id")]
        public virtual UserModel user { get; set; }
        [JsonIgnore]
        [ForeignKey("role_id")]
        public virtual RoleModel role { get; set; }
    }
}
