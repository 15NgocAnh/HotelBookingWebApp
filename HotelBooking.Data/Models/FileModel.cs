using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents a file in the system.
    /// </summary>
    public class FileModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the file.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        [Required]
        [StringLength(50)]
        [RegularExpression("\\w+.(jpg|mp4){1}")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the URL of the file.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the collection of posts associated with this file.
        /// </summary>
        public virtual ICollection<PostModel> Posts { get; set; }
    }
}
