using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.UserAggregate
{
    /// <summary>
    /// Represents a user in the hotel booking system.
    /// This is an aggregate root entity that encapsulates user-related data and behavior.
    /// </summary>
    public class User : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// Gets the user's email address, which serves as the unique identifier for login.
        /// </summary>
        public string Email { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the hashed password for secure authentication.
        /// </summary>
        public string PasswordHash { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the user's first name.
        /// </summary>
        public string FirstName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the user's last name.
        /// </summary>
        public string LastName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the user's phone number, which is optional.
        /// </summary>
        public string? PhoneNumber { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the user account is active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the timestamp of the user's last login.
        /// </summary>
        public DateTime? LastLoginAt { get; private set; }

        /// <summary>
        /// Gets or sets the ID of the user's role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user's role in the system.
        /// </summary>
        public Role Role { get; set; } = default!;

        // Private constructor for EF Core
        private User() { }

        /// <summary>
        /// Initializes a new instance of the User class.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="passwordHash">The hashed password.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="phoneNumber">The user's phone number (optional).</param>
        /// <param name="roleId">The ID of the user's role.</param>
        public User(string email, string passwordHash, string firstName, string lastName, string? phoneNumber, int roleId)
        {
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            RoleId = roleId;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the user's profile information.
        /// </summary>
        /// <param name="firstName">The new first name.</param>
        /// <param name="lastName">The new last name.</param>
        /// <param name="phoneNumber">The new phone number.</param>
        public void UpdateProfile(string firstName, string lastName, string? phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            SetUpdatedAt();
        }

        /// <summary>
        /// Updates the user's basic information.
        /// </summary>
        /// <param name="email">The new email address.</param>
        /// <param name="firstName">The new first name.</param>
        /// <param name="lastName">The new last name.</param>
        /// <param name="phoneNumber">The new phone number.</param>
        /// <param name="roleId">The new role ID.</param>
        public void Update(string email, string firstName, string lastName, string? phoneNumber, int roleId)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            RoleId = roleId;
            SetUpdatedAt();
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="newPasswordHash">The new hashed password.</param>
        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            SetUpdatedAt();
        }

        /// <summary>
        /// Deactivates the user account.
        /// </summary>
        public void Deactivate()
        {
            if (!IsActive)
                return;

            IsActive = false;
            SetUpdatedAt();
        }

        /// <summary>
        /// Activates the user account.
        /// </summary>
        public void Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
            SetUpdatedAt();
        }

        /// <summary>
        /// Updates the timestamp of the user's last login.
        /// </summary>
        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
    }
} 