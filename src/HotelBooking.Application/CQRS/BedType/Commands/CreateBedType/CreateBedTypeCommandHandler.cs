using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Commands.CreateBedType
{
    public class CreateBedTypeCommandHandler : IRequestHandler<CreateBedTypeCommand, Result<int>>
    {
        private readonly IBedTypeRepository _bedTypeRepository;

        public CreateBedTypeCommandHandler(IBedTypeRepository bedTypeRepository)
        {
            _bedTypeRepository = bedTypeRepository ?? throw new ArgumentNullException(nameof(bedTypeRepository));
        }

        public async Task<Result<int>> Handle(CreateBedTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Create new bed type domain entity
                var bedType = new Domain.AggregateModels.BedTypeAggregate.BedType(request.Name);

                // Add bed type to repository
                await _bedTypeRepository.AddAsync(bedType);

                // Return the newly created bed type ID
                return Result<int>.Success(bedType.Id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to create bed type: {ex.Message}");
            }
        }
    }
}