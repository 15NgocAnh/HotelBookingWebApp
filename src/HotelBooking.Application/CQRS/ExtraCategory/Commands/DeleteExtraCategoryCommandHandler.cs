using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraCategory.Commands
{
    public class DeleteExtraCategoryCommandHandler : IRequestHandler<DeleteExtraCategoryCommand, Result>
    {
        private readonly IExtraCategoryRepository _extraCategoryRepository;

        public DeleteExtraCategoryCommandHandler(IExtraCategoryRepository extraCategoryRepository)
        {
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
        }

        public async Task<Result> Handle(DeleteExtraCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing category
                var category = await _extraCategoryRepository.GetByIdAsync(request.Id);
                
                if (category == null)
                {
                    return Result.Failure($"Category with ID {request.Id} not found");
                }

                // Check if category has any items
                var hasItems = await _extraCategoryRepository.HasItemsAsync(request.Id);
                if (hasItems)
                {
                    return Result.Failure("Cannot delete category that has items");
                }
                
                // Delete category
                await _extraCategoryRepository.DeleteAsync(request.Id);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete category: {ex.Message}");
            }
        }
    }
} 