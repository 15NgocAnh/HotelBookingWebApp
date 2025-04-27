using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    public class RolePermissionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        [JsonIgnore]
        [ForeignKey("RoleId")]
        public virtual RoleModel Role { get; set; }

        [JsonIgnore]
        [ForeignKey("PermissionId")]
        public virtual PermissionModel Permission { get; set; }
    }
}
