using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.UserAggregate;

public class UserRole : BaseEntity
{
    public int UserId { get; private set; }
    public int RoleId { get; private set; }    
    
    // Optional navigation (not used in constructor)
    public virtual User User { get; private set; }
    public virtual Role Role { get; private set; }

    private UserRole() { } // For EF

    public UserRole(int userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
} 