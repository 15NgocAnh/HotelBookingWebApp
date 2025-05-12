using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Room.Commands
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Result<int>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoomCommandHandler(
            IRoomRepository roomRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if room number is unique in the building
                if (!await _roomRepository.IsRoomNumberUniqueInBuildingAsync(request.FloorId, request.Name))
                {
                    return Result<int>.Failure("Room number already exists in this building");
                }

                var room = new Domain.AggregateModels.RoomAggregate.Room(
                    request.Name,  // name parameter
                    request.FloorId,       // floorId parameter
                    request.RoomTypeId   // roomTypeId parameter
                );

                await _roomRepository.AddAsync(room);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<int>.Success(room.Id);
            }
            catch (System.Exception ex)
            {
                return Result<int>.Failure($"Failed to create room: {ex.Message}");
            }
        }
    }
} 