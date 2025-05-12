using HotelBooking.Application.CQRS.Commands.RoomTypes;
using HotelBooking.Domain.AggregateModels.AmenityAggregate;
using HotelBooking.Domain.AggregateModels.RoomTypeAggregate;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HotelBooking.Tests.Application.Commands;

public class CreateRoomTypeCommandHandlerTests
{
    private readonly Mock<IRoomTypeRepository> _mockRoomTypeRepository;
    private readonly Mock<IAmenityRepository> _mockAmenityRepository;
    private readonly Mock<ILogger<CreateRoomTypeCommandHandler>> _mockLogger;
    private readonly CreateRoomTypeCommandHandler _handler;

    public CreateRoomTypeCommandHandlerTests()
    {
        _mockRoomTypeRepository = new Mock<IRoomTypeRepository>();
        _mockAmenityRepository = new Mock<IAmenityRepository>();
        _mockLogger = new Mock<ILogger<CreateRoomTypeCommandHandler>>();

        _handler = new CreateRoomTypeCommandHandler(
            _mockRoomTypeRepository.Object,
            _mockAmenityRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateRoomTypeAndReturnId()
    {
        // Arrange
        var command = new CreateRoomTypeCommand
        {
            Name = "Deluxe Room",
            Description = "A luxurious room with a sea view",
            BasePrice = 100,
            Currency = "USD",
            Capacity = 2,
            MaxOccupancy = 3
        };

        _mockRoomTypeRepository.Setup(r => r.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockRoomTypeRepository.Setup(r => r.AddAsync(It.IsAny<RoomType>()))
            .Callback<RoomType>(roomType => roomType.GetType()
                .GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?
                .SetValue(roomType, 1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Data);
        _mockRoomTypeRepository.Verify(r => r.AddAsync(It.IsAny<RoomType>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateName_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateRoomTypeCommand
        {
            Name = "Existing Room Type",
            Description = "This room type already exists",
            BasePrice = 100,
            Currency = "USD",
            Capacity = 2,
            MaxOccupancy = 3
        };

        _mockRoomTypeRepository.Setup(r => r.ExistsAsync("Existing Room Type"))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains($"Room type with name '{command.Name}' already exists", result.Message);
        _mockRoomTypeRepository.Verify(r => r.AddAsync(It.IsAny<RoomType>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithAmenities_ShouldAddAmenitiesToRoomType()
    {
        // Arrange
        var command = new CreateRoomTypeCommand
        {
            Name = "Deluxe Room",
            Description = "A luxurious room with amenities",
            BasePrice = 150,
            Currency = "USD",
            Capacity = 2,
            MaxOccupancy = 3,
            AmenityIds = new List<int> { 1, 2, 3 }
        };

        _mockRoomTypeRepository.Setup(r => r.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        var amenities = new List<Amenity>
        {
            new Amenity("WiFi", new AmenityCategory("Technology")),
            new Amenity("Minibar", new AmenityCategory("Food & Beverage")),
            new Amenity("Air Conditioning", new AmenityCategory("Climate Control"))
        };

        // Set private Id field for amenities (normally done by EF)
        for (int i = 0; i < amenities.Count; i++)
        {
            amenities[i].GetType()
                .GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?
                .SetValue(amenities[i], i + 1);
        }

        _mockAmenityRepository.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(amenities);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRoomTypeRepository.Verify(r => r.AddAsync(It.Is<RoomType>(rt => 
            rt.Name == command.Name && 
            rt.BasePrice.Amount == command.BasePrice)), Times.Once);
        _mockAmenityRepository.Verify(r => r.GetByIdsAsync(It.Is<IEnumerable<int>>(ids => 
            ids.Count() == 3 && ids.Contains(1) && ids.Contains(2) && ids.Contains(3))), Times.Once);
    }
} 