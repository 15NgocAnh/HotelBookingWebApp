using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Room.Queries
{
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, Result<List<RoomDto>>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public GetAllRoomsQueryHandler(
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<RoomDto>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rooms = await _roomRepository.GetAllAsync();
                var roomDtos = _mapper.Map<List<RoomDto>>(rooms);
                return Result<List<RoomDto>>.Success(roomDtos);
            }
            catch (System.Exception ex)
            {
                return Result<List<RoomDto>>.Failure($"Failed to get rooms: {ex.Message}");
            }
        }
    }
} 