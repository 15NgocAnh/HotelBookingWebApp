using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Hotel.Queries
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
    {
        private readonly IHotelRepository _hotelRepository;

        public GetHotelByIdQueryHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository ?? throw new ArgumentNullException(nameof(hotelRepository));
        }

        public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await _hotelRepository.GetByIdAsync(request.Id);
                
                if (hotel == null)
                {
                    return Result<HotelDto>.Failure($"Hotel with ID {request.Id} not found");
                }

                var totalBuildings = await _hotelRepository.CountBuildingsAsync(request.Id);
                
                var hotelDto = new HotelDto
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Description = hotel.Description,
                    Address = hotel.Address,
                    Phone = hotel.Phone,
                    Email = hotel.Email,
                    Website = hotel.Website,
                    TotalBuildings = totalBuildings
                };
                
                return Result<HotelDto>.Success(hotelDto);
            }
            catch (Exception ex)
            {
                return Result<HotelDto>.Failure($"Failed to get hotel: {ex.Message}");
            }
        }
    }
} 