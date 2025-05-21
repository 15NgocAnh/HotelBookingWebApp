using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Common;

public abstract class BaseEntity : BaseEntity<int>
{
}

public abstract class BaseEntity<TId> : IEntity
    where TId : struct, IEquatable<TId>
{
    public TId Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }

    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; } = false;

    public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;

    public void Delete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (!IsDeleted)
            return;

        IsDeleted = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private readonly List<BaseDomainEvent> _domainEvents = [];
    [NotMapped]
    public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RegisterDomainEvent(BaseDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    internal void ClearDomainEvents() => _domainEvents.Clear();
} 