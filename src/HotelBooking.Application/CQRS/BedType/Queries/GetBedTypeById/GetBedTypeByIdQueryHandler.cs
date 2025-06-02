using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.BedType.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Queries.GetBedTypeById
{
    public class GetBedTypeByIdQueryHandler : IRequestHandler<GetBedTypeByIdQuery, Result<BedTypeDto>>
    {
        private readonly IBedTypeRepository _bedTypeRepository;

        public GetBedTypeByIdQueryHandler(IBedTypeRepository bedTypeRepository)
        {
            _bedTypeRepository = bedTypeRepository ?? throw new ArgumentNullException(nameof(bedTypeRepository));
        }

        public async Task<Result<BedTypeDto>> Handle(GetBedTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bedType = await _bedTypeRepository.GetByIdAsync(request.Id);

                if (bedType == null)
                {
                    return Result<BedTypeDto>.Failure($"Bed type with ID {request.Id} not found");
                }

                var bedTypeDto = new BedTypeDto
                {
                    Id = bedType.Id,
                    Name = bedType.Name
                };

                return Result<BedTypeDto>.Success(bedTypeDto);
            }
            catch (Exception ex)
            {
                return Result<BedTypeDto>.Failure($"Failed to get bed type: {ex.Message}");
            }
        }
    }
}