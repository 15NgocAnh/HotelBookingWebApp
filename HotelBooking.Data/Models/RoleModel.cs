using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{

    public class RoleModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        [JsonIgnore]
        public string RoleDesc { get; set; }
        public virtual ICollection<UserRoleModel> UserRole { get; set; }
    }
}
