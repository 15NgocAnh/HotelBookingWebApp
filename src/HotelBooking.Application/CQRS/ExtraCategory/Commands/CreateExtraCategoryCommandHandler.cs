using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.ExtraCategoryAggregate;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraCategory.Commands
{
    public class CreateExtraCategoryCommandHandler : IRequestHandler<CreateExtraCategoryCommand, Result<int>>
    {
        private readonly IExtraCategoryRepository _extraCategoryRepository;

        public CreateExtraCategoryCommandHandler(IExtraCategoryRepository extraCategoryRepository)
        {
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
        }

        public async Task<Result<int>> Handle(CreateExtraCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if category name is unique
                var isNameUnique = await _extraCategoryRepository.IsNameUniqueAsync(request.Name);
                if (!isNameUnique)
                {
                    return Result<int>.Failure($"A category with name '{request.Name}' already exists");
                }

                // Create new category
                var category = new Domain.AggregateModels.ExtraCategoryAggregate.ExtraCategory(request.Name);
                
                // Add to repository
                await _extraCategoryRepository.AddAsync(category);
                
                return Result<int>.Success(category.Id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to create extra category: {ex.Message}");
            }
        }
    }
} 