using HotelBooking.Domain.AggregateModels.RoomAggregate;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HotelBooking.IntegrationTests.Repositories;

public class RoomRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public RoomRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRooms()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();

        // Act
        var rooms = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(rooms);
        Assert.True(rooms.Count >= 2); // We expect at least 2 rooms from seed data
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnRoom()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
        var expectedId = 1;

        // Act
        var room = await repository.GetByIdAsync(expectedId);

        // Assert
        Assert.NotNull(room);
        Assert.Equal(expectedId, room.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
        var invalidId = 9999;

        // Act
        var room = await repository.GetByIdAsync(invalidId);

        // Assert
        Assert.Null(room);
    }

    [Fact]
    public async Task AddAsync_ShouldAddRoom()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        var roomType = await dbContext.RoomTypes.FirstAsync();
        var newRoom = new Room("999", roomType, 200m, "Test Room", 1);

        // Act
        var result = await repository.AddAsync(newRoom);
        await dbContext.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        
        // Verify it was added to the database
        var roomFromDb = await repository.GetByIdAsync(result.Id);
        Assert.NotNull(roomFromDb);
        Assert.Equal("999", roomFromDb.RoomNumber);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldSoftDeleteRoom()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Add a room to delete
        var roomType = await dbContext.RoomTypes.FirstAsync();
        var roomToDelete = new Room("888", roomType, 200m, "Room to Delete", 1);
        await repository.AddAsync(roomToDelete);
        await dbContext.SaveChangesAsync();
        
        // Act
        var result = await repository.DeleteAsync(roomToDelete.Id);
        
        // Assert
        Assert.True(result);
        
        // The room should be soft deleted (IsDeleted = true)
        var deletedRoom = await dbContext.Rooms.FindAsync(roomToDelete.Id);
        Assert.NotNull(deletedRoom);
        Assert.True(deletedRoom.IsDeleted);
        
        // It should not be returned by the repository
        var roomFromRepo = await repository.GetByIdAsync(roomToDelete.Id);
        Assert.Null(roomFromRepo);
    }
}

public class DatabaseFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public DatabaseFixture()
    {
        var services = new ServiceCollection();
        
        // Configure in-memory database
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("HotelBookingTestDb"));
        
        // Add MediatR (required by AppDbContext)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AppDbContext).Assembly));
        
        // Add repositories
        services.AddScoped<IRoomRepository, RoomRepository>();
        
        // Add AutoMapper
        services.AddAutoMapper(cfg => {
            // Add mapper configurations as needed
        });
        
        ServiceProvider = services.BuildServiceProvider();
        
        // Seed the database
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        SeedDatabase(dbContext);
    }

    private void SeedDatabase(AppDbContext dbContext)
    {
        // Add room types
        var standardRoomType = new RoomType("Standard", "Standard room", 100m, 2);
        var deluxeRoomType = new RoomType("Deluxe", "Deluxe room", 200m, 3);
        
        dbContext.RoomTypes.Add(standardRoomType);
        dbContext.RoomTypes.Add(deluxeRoomType);
        dbContext.SaveChanges();
        
        // Add rooms
        var room1 = new Room("101", standardRoomType, 110m, "Standard Room 101", 1);
        var room2 = new Room("201", deluxeRoomType, 220m, "Deluxe Room 201", 2);
        
        dbContext.Rooms.Add(room1);
        dbContext.Rooms.Add(room2);
        dbContext.SaveChanges();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
} 