using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Commands
{
    public class DeleteExtraItemCommandHandler : IRequestHandler<DeleteExtraItemCommand, Result>
    {
        private readonly IExtraItemRepository _extraItemRepository;

        public DeleteExtraItemCommandHandler(IExtraItemRepository extraItemRepository)
        {
            _extraItemRepository = extraItemRepository ?? throw new ArgumentNullException(nameof(extraItemRepository));
        }

        public async Task<Result> Handle(DeleteExtraItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing item
                var item = await _extraItemRepository.GetByIdAsync(request.Id);
                
                if (item == null)
                {
                    return Result.Failure($"Item with ID {request.Id} not found");
                }

                // Check if item is in use
                var isInUse = await _extraItemRepository.IsInUseAsync(request.Id);
                if (isInUse)
                {
                    return Result.Failure("Cannot delete item that is in use");
                }
                
                // Delete item
                await _extraItemRepository.DeleteAsync(request.Id);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete item: {ex.Message}");
            }
        }
    }
} 