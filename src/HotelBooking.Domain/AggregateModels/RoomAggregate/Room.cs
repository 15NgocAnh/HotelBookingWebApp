using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.RoomAggregate;

public class Room(string name, int floorId, int roomTypeId) : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; } = name;
    public RoomStatus Status { get; private set; }
    public int FloorId { get; private init; } = floorId;
    public int RoomTypeId { get; private set; } = roomTypeId;

    public void Update(string name, int roomTypeId)
    {
        Name = name;
        RoomTypeId = roomTypeId;
    }

    public void UpdateStatus(RoomStatus status)
    {
        Status = status;
    }
}
