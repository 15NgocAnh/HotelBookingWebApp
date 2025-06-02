using HotelBooking.Domain.AggregateModels.RoomAggregate;
using HotelBooking.Domain.AggregateModels.RoomAggregate.Events;
using HotelBooking.Domain.Exceptions;
using Xunit;

namespace HotelBooking.UnitTests.Domain;

public class RoomTests
{
    private readonly RoomType _roomType;

    public RoomTests()
    {
        _roomType = new RoomType("Standard", "Standard room", 100, 2);
    }

    [Fact]
    public void CreateRoom_WithValidData_ShouldCreateRoom()
    {
        // Arrange
        var roomNumber = "101";
        var price = 150m;
        var description = "Nice room";
        var floorId = 1;

        // Act
        var room = new Room(roomNumber, _roomType, price, description, floorId);

        // Assert
        Assert.Equal(roomNumber, room.RoomNumber);
        Assert.Equal(_roomType, room.RoomType);
        Assert.Equal(price, room.Price);
        Assert.Equal(description, room.Description);
        Assert.Equal(floorId, room.FloorId);
        Assert.Equal(RoomStatus.Available, room.Status);
        Assert.Single(room.DomainEvents);
        Assert.IsType<RoomCreatedDomainEvent>(room.DomainEvents.First());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateRoom_WithInvalidRoomNumber_ShouldThrowDomainException(string roomNumber)
    {
        // Arrange
        var price = 150m;
        var description = "Nice room";
        var floorId = 1;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new Room(roomNumber, _roomType, price, description, floorId));
        
        Assert.Equal("Room number cannot be empty", exception.Message);
    }

    [Fact]
    public void CreateRoom_WithRoomNumberTooLong_ShouldThrowDomainException()
    {
        // Arrange
        var roomNumber = new string('A', 20); // Create a string longer than the max length
        var price = 150m;
        var description = "Nice room";
        var floorId = 1;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new Room(roomNumber, _roomType, price, description, floorId));
        
        Assert.Equal($"Room number cannot exceed {Domain.Common.Constants.RoomConstants.MaxRoomNumberLength} characters", exception.Message);
    }

    [Fact]
    public void CreateRoom_WithNegativePrice_ShouldThrowDomainException()
    {
        // Arrange
        var roomNumber = "101";
        var price = -10m;
        var description = "Nice room";
        var floorId = 1;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new Room(roomNumber, _roomType, price, description, floorId));
        
        Assert.Equal("Price must be greater than zero", exception.Message);
    }

    [Fact]
    public void CreateRoom_WithZeroPrice_ShouldThrowDomainException()
    {
        // Arrange
        var roomNumber = "101";
        var price = 0m;
        var description = "Nice room";
        var floorId = 1;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new Room(roomNumber, _roomType, price, description, floorId));
        
        Assert.Equal("Price must be greater than zero", exception.Message);
    }

    [Fact]
    public void CreateRoom_WithTooLongDescription_ShouldThrowDomainException()
    {
        // Arrange
        var roomNumber = "101";
        var price = 150m;
        var description = new string('A', 501); // Create a string longer than the max length
        var floorId = 1;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new Room(roomNumber, _roomType, price, description, floorId));
        
        Assert.Equal($"Description cannot exceed {Domain.Common.Constants.RoomConstants.MaxDescriptionLength} characters", exception.Message);
    }

    [Fact]
    public void MarkAsBooked_ShouldChangeStatusToBooked()
    {
        // Arrange
        var room = new Room("101", _roomType, 150m, "Nice room", 1);
        room.ClearDomainEvents(); // Clear the creation event

        // Act
        room.MarkAsBooked();

        // Assert
        Assert.Equal(RoomStatus.Booked, room.Status);
        Assert.Single(room.DomainEvents);
        var @event = room.DomainEvents.First() as RoomStatusChangedDomainEvent;
        Assert.NotNull(@event);
        Assert.Equal(room.Id, @event.RoomId);
        Assert.Equal(RoomStatus.Booked, @event.NewStatus);
    }

    [Fact]
    public void MarkAsBooked_WhenAlreadyBooked_ShouldThrowDomainException()
    {
        // Arrange
        var room = new Room("101", _roomType, 150m, "Nice room", 1);
        room.MarkAsBooked();
        room.ClearDomainEvents(); // Clear previous events

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => room.MarkAsBooked());
        Assert.Equal("Room is not available for booking", exception.Message);
    }

    [Fact]
    public void MarkAsAvailable_ShouldChangeStatusToAvailable()
    {
        // Arrange
        var room = new Room("101", _roomType, 150m, "Nice room", 1);
        room.MarkAsBooked();
        room.ClearDomainEvents(); // Clear previous events

        // Act
        room.MarkAsAvailable();

        // Assert
        Assert.Equal(RoomStatus.Available, room.Status);
        Assert.Single(room.DomainEvents);
        var @event = room.DomainEvents.First() as RoomStatusChangedDomainEvent;
        Assert.NotNull(@event);
        Assert.Equal(room.Id, @event.RoomId);
        Assert.Equal(RoomStatus.Available, @event.NewStatus);
    }

    [Fact]
    public void MarkAsUnderMaintenance_ShouldChangeStatusToUnderMaintenance()
    {
        // Arrange
        var room = new Room("101", _roomType, 150m, "Nice room", 1);
        room.ClearDomainEvents(); // Clear the creation event

        // Act
        room.MarkAsUnderMaintenance();

        // Assert
        Assert.Equal(RoomStatus.UnderMaintenance, room.Status);
        Assert.Single(room.DomainEvents);
        var @event = room.DomainEvents.First() as RoomStatusChangedDomainEvent;
        Assert.NotNull(@event);
        Assert.Equal(room.Id, @event.RoomId);
        Assert.Equal(RoomStatus.UnderMaintenance, @event.NewStatus);
    }
} 