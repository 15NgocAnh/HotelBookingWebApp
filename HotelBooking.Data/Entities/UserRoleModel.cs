using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents the relationship between users and roles in the system.
    /// </summary>
    public class UserRoleModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user role.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with this role.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the role ID associated with this user.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this role.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }

        /// <summary>
        /// Gets or sets the role associated with this user.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("RoleId")]
        public virtual RoleModel Role { get; set; }
    }
}
