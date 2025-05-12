using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Commands
{
    public class CreateExtraItemCommandHandler : IRequestHandler<CreateExtraItemCommand, Result<int>>
    {
        private readonly IExtraItemRepository _extraItemRepository;
        private readonly IExtraCategoryRepository _extraCategoryRepository;

        public CreateExtraItemCommandHandler(
            IExtraItemRepository extraItemRepository,
            IExtraCategoryRepository extraCategoryRepository)
        {
            _extraItemRepository = extraItemRepository ?? throw new ArgumentNullException(nameof(extraItemRepository));
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
        }

        public async Task<Result<int>> Handle(CreateExtraItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if category exists
                var category = await _extraCategoryRepository.GetByIdAsync(request.ExtraCategoryId);
                if (category == null)
                {
                    return Result<int>.Failure($"Category with ID {request.ExtraCategoryId} not found");
                }

                // Check if item name is unique in category
                var isNameUnique = await _extraItemRepository.IsNameUniqueInCategoryAsync(request.ExtraCategoryId, request.Name);
                if (!isNameUnique)
                {
                    return Result<int>.Failure($"An item with name '{request.Name}' already exists in this category");
                }

                // Create new item
                var item = new Domain.AggregateModels.ExtraItemAggregate.ExtraItem(request.ExtraCategoryId, request.Name, request.Price);
                
                // Add to repository
                await _extraItemRepository.AddAsync(item);
                
                return Result<int>.Success(item.Id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to create extra item: {ex.Message}");
            }
        }
    }
} 