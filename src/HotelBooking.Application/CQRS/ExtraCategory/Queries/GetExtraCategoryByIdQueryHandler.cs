using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraCategory.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraCategory.Queries
{
    public class GetExtraCategoryByIdQueryHandler : IRequestHandler<GetExtraCategoryByIdQuery, Result<ExtraCategoryDto>>
    {
        private readonly IExtraCategoryRepository _extraCategoryRepository;
        private readonly IExtraItemRepository _extraItemRepository;

        public GetExtraCategoryByIdQueryHandler(
            IExtraCategoryRepository extraCategoryRepository,
            IExtraItemRepository extraItemRepository)
        {
            _extraCategoryRepository = extraCategoryRepository ?? throw new ArgumentNullException(nameof(extraCategoryRepository));
            _extraItemRepository = extraItemRepository ?? throw new ArgumentNullException(nameof(extraItemRepository));
        }

        public async Task<Result<ExtraCategoryDto>> Handle(GetExtraCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _extraCategoryRepository.GetByIdAsync(request.Id);
                
                if (category == null)
                {
                    return Result<ExtraCategoryDto>.Failure($"Category with ID {request.Id} not found");
                }

                var totalItems = await _extraItemRepository.CountAsync(i => i.ExtraCategoryId == request.Id);
                
                var categoryDto = new ExtraCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    TotalItems = totalItems
                };
                
                return Result<ExtraCategoryDto>.Success(categoryDto);
            }
            catch (Exception ex)
            {
                return Result<ExtraCategoryDto>.Failure($"Failed to get category: {ex.Message}");
            }
        }
    }
} 