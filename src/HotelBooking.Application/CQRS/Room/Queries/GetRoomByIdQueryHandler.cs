using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Room.Queries
{
    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, Result<RoomDto>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public GetRoomByIdQueryHandler(
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<Result<RoomDto>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(request.Id);
                if (room == null)
                {
                    return Result<RoomDto>.Failure("Room not found");
                }

                var roomDto = _mapper.Map<RoomDto>(room);
                return Result<RoomDto>.Success(roomDto);
            }
            catch (System.Exception ex)
            {
                return Result<RoomDto>.Failure($"Failed to get room: {ex.Message}");
            }
        }
    }
} 