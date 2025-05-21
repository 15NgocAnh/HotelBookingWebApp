using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Commands.DeleteBedType
{
    public class DeleteBedTypeCommandHandler : IRequestHandler<DeleteBedTypeCommand, Result>
    {
        private readonly IBedTypeRepository _bedTypeRepository;

        public DeleteBedTypeCommandHandler(IBedTypeRepository bedTypeRepository)
        {
            _bedTypeRepository = bedTypeRepository ?? throw new ArgumentNullException(nameof(bedTypeRepository));
        }

        public async Task<Result> Handle(DeleteBedTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing bed type to check if it exists
                var bedType = await _bedTypeRepository.GetByIdAsync(request.Id);

                if (bedType == null)
                {
                    return Result.Failure($"Bed type with ID {request.Id} not found");
                }

                // Delete bed type
                await _bedTypeRepository.DeleteAsync(request.Id);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete bed type: {ex.Message}");
            }
        }
    }
}