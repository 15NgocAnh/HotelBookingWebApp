namespace HotelBooking.Domain.Common;
public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents);
    Task DispatchAndClearEvents<TId>(IEnumerable<BaseEntity<TId>> entitiesWithEvents) where TId : struct, IEquatable<TId>;
}
