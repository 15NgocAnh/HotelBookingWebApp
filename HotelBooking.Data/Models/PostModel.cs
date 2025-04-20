using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents a post in the system.
    /// </summary>
    public class PostModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the post.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the caption of the post.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the post is active.
        /// </summary>
        public bool IsActived { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the post.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who created the post.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        /// <summary>
        /// Gets or sets the ID of the job associated with the post.
        /// </summary>
        public int? JobId { get; set; } = null;

        /// <summary>
        /// Gets or sets the ID of the file associated with the post.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Gets or sets the file associated with the post.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("FileId")]
        public FileModel File { get; set; }
    }
}
