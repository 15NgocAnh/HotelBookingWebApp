using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Queries
{
    public class GetExtraItemsByCategoryQueryHandler : IRequestHandler<GetExtraItemsByCategoryQuery, Result<List<ExtraItemDto>>>
    {
        private readonly IExtraItemRepository _extraItemRepository;
        private readonly IExtraCategoryRepository _extraCategoryRepository;

        public GetExtraItemsByCategoryQueryHandler(
            IExtraItemRepository extraItemRepository,
            IExtraCategoryRepository extraCategoryRepository)
        {
            _extraItemRepository = extraItemRepository ?? throw new ArgumentNullException(nameof(extraItemRepository));
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
        }

        public async Task<Result<List<ExtraItemDto>>> Handle(GetExtraItemsByCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if category exists
                var category = await _extraCategoryRepository.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    return Result<List<ExtraItemDto>>.Failure($"Category with ID {request.CategoryId} not found");
                }

                var items = await _extraItemRepository.FindAsync(i => i.ExtraCategoryId == request.CategoryId);
                
                var itemDtos = items.Select(i => new ExtraItemDto
                {
                    Id = i.Id,
                    ExtraCategoryId = i.ExtraCategoryId,
                    ExtraCategoryName = category.Name,
                    Name = i.Name,
                    Price = i.Price
                }).ToList();
                
                return Result<List<ExtraItemDto>>.Success(itemDtos);
            }
            catch (Exception ex)
            {
                return Result<List<ExtraItemDto>>.Failure($"Failed to get items: {ex.Message}");
            }
        }
    }
} 