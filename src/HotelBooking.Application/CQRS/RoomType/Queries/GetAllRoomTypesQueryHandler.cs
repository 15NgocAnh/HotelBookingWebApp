using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.RoomType.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBooking.Application.CQRS.RoomType.Queries
{
    public class GetAllRoomTypesQueryHandler : IRequestHandler<GetAllRoomTypesQuery, Result<List<RoomTypeDto>>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;

        public GetAllRoomTypesQueryHandler(
            IRoomTypeRepository roomTypeRepository,
            IMapper mapper)
        {
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<RoomTypeDto>>> Handle(GetAllRoomTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roomTypes = await _roomTypeRepository.GetAllAsync();
                var roomTypeDtos = new List<RoomTypeDto>();

                foreach (var roomType in roomTypes)
                {
                    var roomTypeDto = _mapper.Map<RoomTypeDto>(roomType);
                    roomTypeDto.TotalRooms = await _roomTypeRepository.CountRoomsAsync(roomType.Id);
                    roomTypeDtos.Add(roomTypeDto);
                }

                return Result<List<RoomTypeDto>>.Success(roomTypeDtos);
            }
            catch (System.Exception ex)
            {
                return Result<List<RoomTypeDto>>.Failure($"Failed to get room types: {ex.Message}");
            }
        }
    }
} 