using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Hotel.Commands.UpdateHotel
{
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Result>
    {
        private readonly IHotelRepository _hotelRepository;

        public UpdateHotelCommandHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository ?? throw new ArgumentNullException(nameof(hotelRepository));
        }

        public async Task<Result> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing hotel
                var hotel = await _hotelRepository.GetByIdAsync(request.Id);

                if (hotel == null)
                {
                    return Result.Failure($"Hotel with ID {request.Id} not found");
                }

                // Check if new name is unique (if name is being changed)
                if (hotel.Name != request.Name)
                {
                    var isNameUnique = await _hotelRepository.IsNameUniqueAsync(request.Name);
                    if (!isNameUnique)
                    {
                        return Result.Failure($"A hotel with name '{request.Name}' already exists");
                    }
                }

                // Update hotel
                hotel.Update(
                    request.Name,
                    request.Description,
                    request.Address,
                    request.Phone,
                    request.Email,
                    request.Website
                );

                // Save changes
                await _hotelRepository.UpdateAsync(hotel);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update hotel: {ex.Message}");
            }
        }
    }
}