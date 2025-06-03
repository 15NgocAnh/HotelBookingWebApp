using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Domain.AggregateModels.UserAggregate;

/// <summary>
/// Represents a role in the hotel booking system.
/// This is an aggregate root entity that defines user permissions and access levels.
/// </summary>
public class Role : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// Gets the name of the role.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the optional description of the role.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the collection of users associated with this role.
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();

    /// <summary>
    /// Initializes a new instance of the Role class.
    /// Required for EF Core.
    /// </summary>
    public Role()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Role class with specified name and description.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    /// <param name="description">The description of the role.</param>
    /// <exception cref="DomainException">Thrown when the name is empty or exceeds 50 characters,
    /// or when the description exceeds 200 characters.</exception>
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

    /// <summary>
    /// Updates the role's name and description.
    /// </summary>
    /// <param name="name">The new name of the role.</param>
    /// <param name="description">The new description of the role.</param>
    /// <exception cref="DomainException">Thrown when the name is empty or exceeds 50 characters,
    /// or when the description exceeds 200 characters.</exception>
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