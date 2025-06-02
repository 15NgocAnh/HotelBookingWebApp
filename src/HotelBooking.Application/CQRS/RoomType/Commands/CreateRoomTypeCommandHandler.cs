using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.RoomTypeAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.RoomType.Commands
{
    public class CreateRoomTypeCommandHandler : IRequestHandler<CreateRoomTypeCommand, Result<int>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoomTypeCommandHandler(
            IRoomTypeRepository roomTypeRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if room type name is unique
                if (!await _roomTypeRepository.IsNameUniqueAsync(request.Name))
                {
                    return Result<int>.Failure("Room type name already exists");
                }

                // Convert DTOs to domain entities
                var bedTypeSetupDetails = request.BedTypeSetupDetails?.Select(dto => 
                    new BedTypeSetupDetail(dto.BedTypeId, dto.BedTypeName, dto.Quantity)).ToList() ?? new List<BedTypeSetupDetail>();

                var amenitySetupDetails = request.AmenitySetupDetails?.Select(dto => 
                    new AmenitySetupDetail(dto.AmenityId, dto.AmenityName, dto.Quantity)).ToList() ?? new List<AmenitySetupDetail>();

                var roomType = new Domain.AggregateModels.RoomTypeAggregate.RoomType(
                    request.Name,
                    request.Price,
                    bedTypeSetupDetails,
                    amenitySetupDetails
                );

                await _roomTypeRepository.AddAsync(roomType);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<int>.Success(roomType.Id);
            }
            catch (System.Exception ex)
            {
                return Result<int>.Failure($"Failed to create room type: {ex.Message}");
            }
        }
    }
} 