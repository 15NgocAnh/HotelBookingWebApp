namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Base class for all entity models in the application.
    /// Provides common properties for tracking entity changes and soft delete functionality.
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// Gets or sets the ID of the user who last modified the entity.
        /// </summary>
        public int ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the entity.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is soft deleted.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
