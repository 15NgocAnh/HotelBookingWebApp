using MediatR;

namespace HotelBooking.Domain.Common;
public class DomainEventDispatcher(IPublisher _publisher) : IDomainEventDispatcher
{
    public async Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents)
    {
        await DispatchAndClearEvents<int>(entitiesWithEvents);
    }

    public async Task DispatchAndClearEvents<TId>(IEnumerable<BaseEntity<TId>> entitiesWithEvents) where TId : struct, IEquatable<TId>
    {
        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();
            foreach (var domainEvent in events)
            {
                await _publisher.Publish(domainEvent);
            }
        }
    }
}
