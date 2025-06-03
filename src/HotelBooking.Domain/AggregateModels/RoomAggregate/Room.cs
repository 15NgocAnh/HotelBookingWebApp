using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.RoomAggregate;

/// <summary>
/// Represents a room in the hotel.
/// This is an aggregate root entity that manages room information and its current status.
/// </summary>
public class Room(string name, int floorId, int roomTypeId) : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// Gets the name or number of the room.
    /// </summary>
    public string Name { get; private set; } = name;

    /// <summary>
    /// Gets the current status of the room.
    /// </summary>
    public RoomStatus Status { get; private set; }

    /// <summary>
    /// Gets the ID of the floor where this room is located.
    /// </summary>
    public int FloorId { get; private init; } = floorId;

    /// <summary>
    /// Gets the ID of the room type that defines this room's characteristics.
    /// </summary>
    public int RoomTypeId { get; private set; } = roomTypeId;

    /// <summary>
    /// Updates the room's basic information.
    /// </summary>
    /// <param name="name">The new name or number for the room.</param>
    /// <param name="roomTypeId">The new room type ID.</param>
    public void Update(string name, int roomTypeId)
    {
        Name = name;
        RoomTypeId = roomTypeId;
    }

    /// <summary>
    /// Updates the current status of the room.
    /// </summary>
    /// <param name="status">The new status for the room.</param>
    public void UpdateStatus(RoomStatus status)
    {
        Status = status;
    }
}
