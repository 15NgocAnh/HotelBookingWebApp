using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.RoomManagement.Queries
{
    public class GetRoomTreeQueryHandler : IRequestHandler<GetRoomTreeQuery, Result<List<object>>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IRoomRepository _roomRepository;

        public GetRoomTreeQueryHandler(
            IHotelRepository hotelRepository,
            IBuildingRepository buildingRepository,
            IRoomRepository roomRepository)
        {
            _hotelRepository = hotelRepository;
            _buildingRepository = buildingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<Result<List<object>>> Handle(GetRoomTreeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotels = await _hotelRepository.GetAllAsync();
                var treeData = new List<object>();

                foreach (var hotel in hotels)
                {
                    var hotelNode = new
                    {
                        id = $"hotel_{hotel.Id}",
                        text = hotel.Name,
                        type = "hotel",
                        children = new List<object>()
                    };

                    var buildings = await _buildingRepository.GetBuildingsByHotelAsync(hotel.Id);
                    foreach (var building in buildings)
                    {
                        var buildingNode = new
                        {
                            id = $"building_{building.Id}",
                            text = building.Name,
                            type = "building",
                            children = new List<object>()
                        };

                        var floors = building.Floors;
                        foreach (var floor in floors)
                        {
                            var floorNode = new
                            {
                                id = $"floor_{floor.Id}",
                                text = $"Floor {floor.Number}",
                                type = "floor",
                                children = new List<object>()
                            };

                            var rooms = await _roomRepository.GetRoomsByFloorIdAsync(floor.Id);
                            foreach (var room in rooms)
                            {
                                var roomNode = new
                                {
                                    id = $"room_{room.Id}",
                                    text = room.Name,
                                    type = "room",
                                    data = new { status = room.Status }
                                };
                                floorNode.children.Add(roomNode);
                            }

                            buildingNode.children.Add(floorNode);
                        }

                        hotelNode.children.Add(buildingNode);
                    }

                    treeData.Add(hotelNode);
                }

                return Result<List<object>>.Success(treeData);
            }
            catch (Exception ex)
            {
                return Result<List<object>>.Failure($"Failed to get room tree: {ex.Message}");
            }
        }
    }
} 