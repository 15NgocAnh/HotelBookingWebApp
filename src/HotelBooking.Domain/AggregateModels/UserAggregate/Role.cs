using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Domain.AggregateModels.UserAggregate;

public class Role : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ICollection<User> Users { get; set; } = new List<User>();

    public Role()
    {
    }

    public Role(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name cannot be empty");
            
        if (name.Length > 50)
            throw new DomainException("Role name cannot exceed 50 characters");
            
        if (description?.Length > 200)
            throw new DomainException("Role description cannot exceed 200 characters");
            
        Name = name;
        Description = description;
    }

    public void Update(string name, string description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name cannot be empty");
            
        if (name.Length > 50)
            throw new DomainException("Role name cannot exceed 50 characters");
            
        if (description?.Length > 200)
            throw new DomainException("Role description cannot exceed 200 characters");
            
        Name = name;
        Description = description;
    }
} 