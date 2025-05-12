using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Commands
{
    public class UpdateExtraItemCommandHandler : IRequestHandler<UpdateExtraItemCommand, Result>
    {
        private readonly IExtraItemRepository _extraItemRepository;
        private readonly IExtraCategoryRepository _extraCategoryRepository;

        public UpdateExtraItemCommandHandler(
            IExtraItemRepository extraItemRepository,
            IExtraCategoryRepository extraCategoryRepository)
        {
            _extraItemRepository = extraItemRepository ?? throw new ArgumentNullException(nameof(extraItemRepository));
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
        }

        public async Task<Result> Handle(UpdateExtraItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing item
                var item = await _extraItemRepository.GetByIdAsync(request.Id);
                
                if (item == null)
                {
                    return Result.Failure($"Item with ID {request.Id} not found");
                }

                // Check if category exists
                var category = await _extraCategoryRepository.GetByIdAsync(request.ExtraCategoryId);
                if (category == null)
                {
                    return Result.Failure($"Category with ID {request.ExtraCategoryId} not found");
                }

                // Check if item name is unique in category (if name is being changed)
                if (item.Name != request.Name || item.ExtraCategoryId != request.ExtraCategoryId)
                {
                    var isNameUnique = await _extraItemRepository.IsNameUniqueInCategoryAsync(request.ExtraCategoryId, request.Name);
                    if (!isNameUnique)
                    {
                        return Result.Failure($"An item with name '{request.Name}' already exists in this category");
                    }
                }
                
                // Update item
                item.Update(request.ExtraCategoryId, request.Name, request.Price);
                
                // Save changes
                await _extraItemRepository.UpdateAsync(item);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update item: {ex.Message}");
            }
        }
    }
} 