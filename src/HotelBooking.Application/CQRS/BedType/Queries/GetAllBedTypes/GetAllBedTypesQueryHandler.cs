using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.BedType.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Queries.GetAllBedTypes
{
    public class GetAllBedTypesQueryHandler : IRequestHandler<GetAllBedTypesQuery, Result<List<BedTypeDto>>>
    {
        private readonly IBedTypeRepository _bedTypeRepository;

        public GetAllBedTypesQueryHandler(IBedTypeRepository bedTypeRepository)
        {
            _bedTypeRepository = bedTypeRepository ?? throw new ArgumentNullException(nameof(bedTypeRepository));
        }

        public async Task<Result<List<BedTypeDto>>> Handle(GetAllBedTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bedTypes = await _bedTypeRepository.GetAllAsync();

                var bedTypeDtos = bedTypes.Select(b => new BedTypeDto
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList();

                return Result<List<BedTypeDto>>.Success(bedTypeDtos);
            }
            catch (Exception ex)
            {
                return Result<List<BedTypeDto>>.Failure($"Failed to get bed types: {ex.Message}");
            }
        }
    }
}