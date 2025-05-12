using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Hotel.Commands
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<int>>
    {
        private readonly IHotelRepository _hotelRepository;

        public CreateHotelCommandHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository ?? throw new ArgumentNullException(nameof(hotelRepository));
        }

        public async Task<Result<int>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if hotel name is unique
                var isNameUnique = await _hotelRepository.IsNameUniqueAsync(request.Name);
                if (!isNameUnique)
                {
                    return Result<int>.Failure($"A hotel with name '{request.Name}' already exists");
                }

                // Create new hotel
                var hotel = new Domain.AggregateModels.HotelAggregate.Hotel(
                    request.Name,
                    request.Description,
                    request.Address,
                    request.Phone,
                    request.Email,
                    request.Website
                );
                
                // Add to repository
                await _hotelRepository.AddAsync(hotel);
                
                return Result<int>.Success(hotel.Id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to create hotel: {ex.Message}");
            }
        }
    }
} 