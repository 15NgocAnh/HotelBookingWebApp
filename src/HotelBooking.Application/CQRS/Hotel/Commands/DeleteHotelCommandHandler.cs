using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Hotel.Commands
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, Result>
    {
        private readonly IHotelRepository _hotelRepository;

        public DeleteHotelCommandHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository ?? throw new ArgumentNullException(nameof(hotelRepository));
        }

        public async Task<Result> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing hotel
                var hotel = await _hotelRepository.GetByIdAsync(request.Id);
                
                if (hotel == null)
                {
                    return Result.Failure($"Hotel with ID {request.Id} not found");
                }

                // Check if hotel has any buildings
                var hasBuildings = await _hotelRepository.HasBuildingsAsync(request.Id);
                if (hasBuildings)
                {
                    return Result.Failure("Cannot delete hotel that has buildings");
                }
                
                // Delete hotel
                await _hotelRepository.DeleteAsync(request.Id);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete hotel: {ex.Message}");
            }
        }
    }
} 