using AutoMapper;
using HotelBooking.Application.Features.Rooms.Commands.CreateRoom;
using HotelBooking.Domain.AggregateModels.RoomAggregate;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests.Application;

public class CreateRoomCommandHandlerTests
{
    private readonly Mock<IRoomRepository> _mockRoomRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public CreateRoomCommandHandlerTests()
    {
        _mockRoomRepository = new Mock<IRoomRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        
        _mockRoomRepository.Setup(repo => repo.UnitOfWork).Returns(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnSuccessResponse()
    {
        // Arrange
        var command = new CreateRoomCommand
        {
            RoomNumber = "101",
            RoomTypeId = 1,
            Price = 150m,
            Description = "Nice room",
            FloorId = 1
        };

        var roomType = new RoomType("Standard", "Standard room", 100, 2);
        var room = new Room(command.RoomNumber, roomType, command.Price, command.Description, command.FloorId);
        room.GetType().GetProperty("Id").SetValue(room, 1);

        _mockRoomRepository.Setup(repo => repo.UnitOfWork.GetRoomTypeById(command.RoomTypeId))
            .ReturnsAsync(roomType);
        _mockRoomRepository.Setup(repo => repo.AddAsync(It.IsAny<Room>()))
            .ReturnsAsync(room);

        var handler = new CreateRoomCommandHandler(_mockRoomRepository.Object, _mockMapper.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(1, result.Data);
        Assert.Equal("Room created successfully", result.Message);
        
        _mockRoomRepository.Verify(repo => repo.AddAsync(It.IsAny<Room>()), Times.Once);
        _mockRoomRepository.Verify(repo => repo.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRoomType_ShouldReturnFailureResponse()
    {
        // Arrange
        var command = new CreateRoomCommand
        {
            RoomNumber = "101",
            RoomTypeId = 999, // Non-existent room type ID
            Price = 150m,
            Description = "Nice room",
            FloorId = 1
        };

        _mockRoomRepository.Setup(repo => repo.UnitOfWork.GetRoomTypeById(command.RoomTypeId))
            .ReturnsAsync((RoomType)null);

        var handler = new CreateRoomCommandHandler(_mockRoomRepository.Object, _mockMapper.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Room type with ID {command.RoomTypeId} not found", result.Message);
        
        _mockRoomRepository.Verify(repo => repo.AddAsync(It.IsAny<Room>()), Times.Never);
        _mockRoomRepository.Verify(repo => repo.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_ShouldReturnFailureResponse()
    {
        // Arrange
        var command = new CreateRoomCommand
        {
            RoomNumber = "101",
            RoomTypeId = 1,
            Price = 150m,
            Description = "Nice room",
            FloorId = 1
        };

        var roomType = new RoomType("Standard", "Standard room", 100, 2);

        _mockRoomRepository.Setup(repo => repo.UnitOfWork.GetRoomTypeById(command.RoomTypeId))
            .ReturnsAsync(roomType);
        _mockRoomRepository.Setup(repo => repo.AddAsync(It.IsAny<Room>()))
            .ThrowsAsync(new Exception("Database error"));

        var handler = new CreateRoomCommandHandler(_mockRoomRepository.Object, _mockMapper.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Error creating room: Database error", result.Message);
    }
} 