using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.RoomTypeAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.RoomType.Commands
{
    public class UpdateRoomTypeCommandHandler : IRequestHandler<UpdateRoomTypeCommand, Result>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoomTypeCommandHandler(
            IRoomTypeRepository roomTypeRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var roomType = await _roomTypeRepository.GetByIdAsync(request.Id);
                if (roomType == null)
                {
                    return Result.Failure("Room type not found");
                }

                // Check if room type name is unique (excluding current room type)
                if (!await _roomTypeRepository.IsNameUniqueAsync(request.Name))
                {
                    var existingRoomType = await _roomTypeRepository.GetByNameAsync(request.Name);
                    if (existingRoomType != null && existingRoomType.Id != request.Id)
                    {
                        return Result.Failure("Room type name already exists");
                    }
                }

                // Convert DTOs to domain entities
                var bedTypeSetupDetails = request.BedTypeSetupDetails?.Select(dto => 
                    new BedTypeSetupDetail(dto.BedTypeId, dto.BedTypeName, dto.Quantity)).ToList() ?? new List<BedTypeSetupDetail>();

                var amenitySetupDetails = request.AmenitySetupDetails?.Select(dto => 
                    new AmenitySetupDetail(dto.AmenityId, dto.AmenityName, dto.Quantity)).ToList() ?? new List<AmenitySetupDetail>();

                roomType.Update(
                    request.Name,
                    request.Price,
                    bedTypeSetupDetails,
                    amenitySetupDetails
                );

                await _roomTypeRepository.UpdateAsync(roomType);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Failed to update room type: {ex.Message}");
            }
        }
    }
} 