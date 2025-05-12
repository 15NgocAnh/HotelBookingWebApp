using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraCategory.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraCategory.Queries
{
    public class GetAllExtraCategoriesQueryHandler : IRequestHandler<GetAllExtraCategoriesQuery, Result<List<ExtraCategoryDto>>>
    {
        private readonly IExtraCategoryRepository _extraCategoryRepository;
        private readonly IExtraItemRepository _extraItemRepository;

        public GetAllExtraCategoriesQueryHandler(
            IExtraCategoryRepository extraCategoryRepository,
            IExtraItemRepository extraItemRepository)
        {
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
            _extraItemRepository = extraItemRepository ?? throw new ArgumentNullException(nameof(extraItemRepository));
        }

        public async Task<Result<List<ExtraCategoryDto>>> Handle(GetAllExtraCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _extraCategoryRepository.GetAllAsync();
                var items = await _extraItemRepository.GetAllAsync();
                
                var categoryDtos = categories.Select(c => new ExtraCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TotalItems = items.Count(i => i.ExtraCategoryId == c.Id)
                }).ToList();
                
                return Result<List<ExtraCategoryDto>>.Success(categoryDtos);
            }
            catch (Exception ex)
            {
                return Result<List<ExtraCategoryDto>>.Failure($"Failed to get categories: {ex.Message}");
            }
        }
    }
} 