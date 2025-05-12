using HotelBooking.Domain.AggregateModels.RoomTypeAggregate;
using HotelBooking.Domain.AggregateModels.RoomTypeAggregate.Events;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Domain.ValueObjects;
using Xunit;

namespace HotelBooking.Tests.Domain;

public class RoomTypeAggregateTests
{
    [Fact]
    public void Create_ValidParameters_ShouldSucceed()
    {
        // Arrange
        string name = "Deluxe Room";
        string description = "A luxurious room with a sea view";
        Money basePrice = new Money(100, "USD");
        int capacity = 2;
        int maxOccupancy = 3;

        // Act
        var roomType = new RoomType(name, description, basePrice, capacity, maxOccupancy);

        // Assert
        Assert.Equal(name, roomType.Name);
        Assert.Equal(description, roomType.Description);
        Assert.Equal(basePrice, roomType.BasePrice);
        Assert.Equal(capacity, roomType.Capacity);
        Assert.Equal(maxOccupancy, roomType.MaxOccupancy);
        Assert.True(roomType.IsActive);
        Assert.Single(roomType.DomainEvents);
        Assert.IsType<RoomTypeCreatedDomainEvent>(roomType.DomainEvents[0]);
    }

    [Fact]
    public void Create_EmptyName_ShouldThrowDomainException()
    {
        // Arrange
        string name = "";
        string description = "A luxurious room with a sea view";
        Money basePrice = new Money(100, "USD");
        int capacity = 2;
        int maxOccupancy = 3;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new RoomType(name, description, basePrice, capacity, maxOccupancy));
        Assert.Equal("Room type name cannot be empty", exception.Message);
    }

    [Fact]
    public void Create_InvalidCapacity_ShouldThrowDomainException()
    {
        // Arrange
        string name = "Deluxe Room";
        string description = "A luxurious room with a sea view";
        Money basePrice = new Money(100, "USD");
        int capacity = 0; // Invalid capacity
        int maxOccupancy = 3;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new RoomType(name, description, basePrice, capacity, maxOccupancy));
        Assert.Equal("Capacity must be greater than zero", exception.Message);
    }

    [Fact]
    public void Create_MaxOccupancyLessThanCapacity_ShouldThrowDomainException()
    {
        // Arrange
        string name = "Deluxe Room";
        string description = "A luxurious room with a sea view";
        Money basePrice = new Money(100, "USD");
        int capacity = 3;
        int maxOccupancy = 2; // Invalid: less than capacity

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new RoomType(name, description, basePrice, capacity, maxOccupancy));
        Assert.Equal("Max occupancy cannot be less than capacity", exception.Message);
    }

    [Fact]
    public void Update_ValidParameters_ShouldSucceed()
    {
        // Arrange
        var roomType = CreateValidRoomType();
        string newName = "Super Deluxe Room";
        string newDescription = "An even more luxurious room";
        Money newBasePrice = new Money(150, "USD");
        int newCapacity = 3;
        int newMaxOccupancy = 4;

        // Act
        roomType.Update(newName, newDescription, newBasePrice, newCapacity, newMaxOccupancy);

        // Assert
        Assert.Equal(newName, roomType.Name);
        Assert.Equal(newDescription, roomType.Description);
        Assert.Equal(newBasePrice, roomType.BasePrice);
        Assert.Equal(newCapacity, roomType.Capacity);
        Assert.Equal(newMaxOccupancy, roomType.MaxOccupancy);
        Assert.Equal(2, roomType.DomainEvents.Count);
        Assert.IsType<RoomTypeUpdatedDomainEvent>(roomType.DomainEvents[1]);
    }

    [Fact]
    public void Deactivate_ActiveRoomType_ShouldDeactivate()
    {
        // Arrange
        var roomType = CreateValidRoomType();
        Assert.True(roomType.IsActive);

        // Act
        roomType.Deactivate();

        // Assert
        Assert.False(roomType.IsActive);
        Assert.Equal(2, roomType.DomainEvents.Count);
        Assert.IsType<RoomTypeDeactivatedDomainEvent>(roomType.DomainEvents[1]);
    }

    [Fact]
    public void Activate_InactiveRoomType_ShouldActivate()
    {
        // Arrange
        var roomType = CreateValidRoomType();
        roomType.Deactivate();
        Assert.False(roomType.IsActive);

        // Act
        roomType.Activate();

        // Assert
        Assert.True(roomType.IsActive);
        Assert.Equal(3, roomType.DomainEvents.Count);
        Assert.IsType<RoomTypeActivatedDomainEvent>(roomType.DomainEvents[2]);
    }

    [Fact]
    public void UpdateBasePrice_ValidPrice_ShouldUpdateAndRaiseEvent()
    {
        // Arrange
        var roomType = CreateValidRoomType();
        var oldPrice = roomType.BasePrice;
        var newPrice = new Money(150, "USD");

        // Act
        roomType.UpdateBasePrice(newPrice);

        // Assert
        Assert.Equal(newPrice, roomType.BasePrice);
        Assert.Equal(2, roomType.DomainEvents.Count);
        var priceChangedEvent = Assert.IsType<RoomTypeBasePriceChangedDomainEvent>(roomType.DomainEvents[1]);
        Assert.Equal(oldPrice, priceChangedEvent.OldPrice);
    }

    private RoomType CreateValidRoomType()
    {
        return new RoomType(
            "Deluxe Room",
            "A luxurious room with a sea view",
            new Money(100, "USD"),
            2,
            3);
    }
} 