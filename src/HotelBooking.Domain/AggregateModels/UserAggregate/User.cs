using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.UserAggregate
{
    public class User : BaseEntity, IAggregateRoot
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        private readonly List<UserRole> _userRoles = [];
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

        public User() { } // For EF Core

        public User(string email, string passwordHash, string firstName, string lastName, string phoneNumber)
        {
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            _userRoles = [];
        }

        public void UpdateProfile(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void AddRole(Role role)
        {
            if (!UserRoles.Any(ur => ur.RoleId == role.Id))
            {
                _userRoles.Add(new UserRole(this.Id, role.Id));
            }
        }

        public void RemoveRole(Role role)
        {
            var userRole = UserRoles.FirstOrDefault(ur => ur.RoleId == role.Id);
            if (userRole != null)
            {
                _userRoles.Remove(userRole);
            }
        }

        public bool HasRole(string roleName)
        {
            return UserRoles.Any(ur => ur.Role.Name == roleName);
        }
    }
} 