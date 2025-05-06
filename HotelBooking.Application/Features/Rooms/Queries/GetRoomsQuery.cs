using AutoMapper;
using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.Features.Rooms.Queries;

public class GetRoomsQuery : IQuery<List<RoomDTO>>
{
}

public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, BaseResponse<List<RoomDTO>>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomsQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<RoomDTO>>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetAllAsync();
        var roomDtos = _mapper.Map<List<RoomDTO>>(rooms);
        
        return new BaseResponse<List<RoomDTO>>(roomDtos);
    }
} 