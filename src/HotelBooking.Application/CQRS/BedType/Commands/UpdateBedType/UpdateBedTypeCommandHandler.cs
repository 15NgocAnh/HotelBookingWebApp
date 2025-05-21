using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Commands.UpdateBedType
{
    public class UpdateBedTypeCommandHandler : IRequestHandler<UpdateBedTypeCommand, Result>
    {
        private readonly IBedTypeRepository _bedTypeRepository;

        public UpdateBedTypeCommandHandler(IBedTypeRepository bedTypeRepository)
        {
            _bedTypeRepository = bedTypeRepository ?? throw new ArgumentNullException(nameof(bedTypeRepository));
        }

        public async Task<Result> Handle(UpdateBedTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing bed type
                var bedType = await _bedTypeRepository.GetByIdAsync(request.Id);

                if (bedType == null)
                {
                    return Result.Failure($"Bed type with ID {request.Id} not found");
                }

                // Update bed type properties
                bedType.Update(request.Name);

                // Save changes
                await _bedTypeRepository.UpdateAsync(bedType);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update bed type: {ex.Message}");
            }
        }
    }
}