using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Queries
{
    public class GetAllExtraItemsQueryHandler : IRequestHandler<GetAllExtraItemsQuery, Result<List<ExtraItemDto>>>
    {
        private readonly IExtraCategoryRepository _extraCategoryRepository;
        private readonly IExtraItemRepository _extraItemRepository;

        public GetAllExtraItemsQueryHandler(
            IExtraCategoryRepository extraCategoryRepository,
            IExtraItemRepository extraItemRepository)
        {
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
            _extraItemRepository = extraItemRepository ?? throw new ArgumentNullException(nameof(extraItemRepository));
        }

        public async Task<Result<List<ExtraItemDto>>> Handle(GetAllExtraItemsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var items = await _extraItemRepository.GetAllAsync();
                var categories = await _extraCategoryRepository.GetAllAsync();

                var extraItemDtos = items.Select(c => new ExtraItemDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price,
                    ExtraCategoryId = c.ExtraCategoryId,
                    ExtraCategoryName = categories.Where(x => x.Id == c.ExtraCategoryId).Select(x => x.Name).FirstOrDefault() ?? string.Empty,
                }).ToList();

                return Result<List<ExtraItemDto>>.Success(extraItemDtos);
            }
            catch (Exception ex)
            {
                return Result<List<ExtraItemDto>>.Failure($"Failed to get categories: {ex.Message}");
            }
        }
    }
}