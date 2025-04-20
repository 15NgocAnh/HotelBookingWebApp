using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents a role in the system.
    /// </summary>
    public class RoleModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the role.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Role name must contain at least 1 character and maximum to 50 character")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        [StringLength(200, ErrorMessage = "Description must not exceed 200 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of user roles associated with this role.
        /// </summary>
        [JsonIgnore]
        public ICollection<UserRoleModel> UserRoles { get; set; }
    }
}
