using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.RoomManagement.Queries
{
    public class SearchRoomQueryHandler : IRequestHandler<SearchRoomQuery, Result<int>>
    {
        private readonly IRoomRepository _roomRepository;

        public SearchRoomQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Result<int>> Handle(SearchRoomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Term))
                {
                    return Result<int>.Failure("Search term is required");
                }

                var room = await _roomRepository.GetByRoomNumberAsync(request.Term);
                if (room == null)
                {
                    return Result<int>.Failure("Room not found");
                }

                return Result<int>.Success(room.Id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to search room: {ex.Message}");
            }
        }
    }
} 