using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraCategory.Commands
{
    public class UpdateExtraCategoryCommandHandler : IRequestHandler<UpdateExtraCategoryCommand, Result>
    {
        private readonly IExtraCategoryRepository _extraCategoryRepository;

        public UpdateExtraCategoryCommandHandler(IExtraCategoryRepository extraCategoryRepository)
        {
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
        }

        public async Task<Result> Handle(UpdateExtraCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing category
                var category = await _extraCategoryRepository.GetByIdAsync(request.Id);
                
                if (category == null)
                {
                    return Result.Failure($"Category with ID {request.Id} not found");
                }

                // Check if new name is unique (if name is being changed)
                if (category.Name != request.Name)
                {
                    var isNameUnique = await _extraCategoryRepository.IsNameUniqueAsync(request.Name);
                    if (!isNameUnique)
                    {
                        return Result.Failure($"A category with name '{request.Name}' already exists");
                    }
                }
                
                // Update category
                category.Update(request.Name);
                
                // Save changes
                await _extraCategoryRepository.UpdateAsync(category);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update category: {ex.Message}");
            }
        }
    }
} 