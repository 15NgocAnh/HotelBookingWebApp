using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Queries
{
    public class GetExtraItemByIdQueryHandler : IRequestHandler<GetExtraItemByIdQuery, Result<ExtraItemDto>>
    {
        private readonly IExtraItemRepository _extraItemRepository;
        private readonly IExtraCategoryRepository _extraCategoryRepository;

        public GetExtraItemByIdQueryHandler(
            IExtraItemRepository extraItemRepository,
            IExtraCategoryRepository extraCategoryRepository)
        {
            _extraItemRepository = extraItemRepository ?? throw new ArgumentNullException(nameof(extraItemRepository));
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
        }

        public async Task<Result<ExtraItemDto>> Handle(GetExtraItemByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _extraItemRepository.GetByIdAsync(request.Id);
                
                if (item == null)
                {
                    return Result<ExtraItemDto>.Failure($"Item with ID {request.Id} not found");
                }

                var category = await _extraCategoryRepository.GetByIdAsync(item.ExtraCategoryId);
                if (category == null)
                {
                    return Result<ExtraItemDto>.Failure($"Category with ID {item.ExtraCategoryId} not found");
                }
                
                var itemDto = new ExtraItemDto
                {
                    Id = item.Id,
                    ExtraCategoryId = item.ExtraCategoryId,
                    ExtraCategoryName = category.Name,
                    Name = item.Name,
                    Price = item.Price
                };
                
                return Result<ExtraItemDto>.Success(itemDto);
            }
            catch (Exception ex)
            {
                return Result<ExtraItemDto>.Failure($"Failed to get item: {ex.Message}");
            }
        }
    }
} 