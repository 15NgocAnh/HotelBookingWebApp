using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents a JWT token in the system.
    /// </summary>
    public class JWTModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the JWT token.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the hashed value of the JWT token.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string TokenHashValue { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the JWT token.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        /// Gets or sets the Firebase Cloud Messaging token.
        /// </summary>
        public string? FcmToken { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user associated with this token.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this token.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("UserId")]
        public UserModel User { get; set; }
    }
}
