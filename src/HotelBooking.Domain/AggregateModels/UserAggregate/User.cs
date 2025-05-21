namespace HotelBooking.Domain.AggregateModels.UserAggregate
{
    public class User : BaseEntity, IAggregateRoot
    {
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string? PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = default!;

        private User() { } // For EF Core

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

        public void UpdateProfile(string firstName, string lastName, string? phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            SetUpdatedAt();
        }

        public void Update(string email, string firstName, string lastName, string? phoneNumber, int roleId)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            RoleId = roleId;
            SetUpdatedAt();
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            if (!IsActive)
                return;

            IsActive = false;
            SetUpdatedAt();
        }

        public void Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
            SetUpdatedAt();
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
    }
} 