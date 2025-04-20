using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents a notification in the system.
    /// </summary>
    public class NotificationModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the notification.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the content of the notification.
        /// </summary>
        public string NotifiContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the notification has been read.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the notification has been accepted.
        /// </summary>
        public bool IsAccept { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who sent the notification.
        /// </summary>
        public int FromUserNotifiId { get; set; }

        /// <summary>
        /// Gets or sets the user who sent the notification.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("FromUserNotifiId")]
        public UserModel FromUserNotification { get; set; }
    }
}
