using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.RoomType.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBooking.Application.CQRS.RoomType.Queries
{
    public class GetRoomTypeByIdQueryHandler : IRequestHandler<GetRoomTypeByIdQuery, Result<RoomTypeDto>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;

        public GetRoomTypeByIdQueryHandler(
            IRoomTypeRepository roomTypeRepository,
            IMapper mapper)
        {
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
        }

        public async Task<Result<RoomTypeDto>> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roomType = await _roomTypeRepository.GetByIdAsync(request.Id);
                if (roomType == null)
                {
                    return Result<RoomTypeDto>.Failure("Room type not found");
                }

                var roomTypeDto = _mapper.Map<RoomTypeDto>(roomType);
                roomTypeDto.TotalRooms = await _roomTypeRepository.CountRoomsAsync(request.Id);
                
                return Result<RoomTypeDto>.Success(roomTypeDto);
            }
            catch (System.Exception ex)
            {
                return Result<RoomTypeDto>.Failure($"Failed to get room type: {ex.Message}");
            }
        }
    }
} 