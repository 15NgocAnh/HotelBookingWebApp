using HotelBooking.Application.CQRS.Hotel.DTOs;

namespace HotelBooking.Application.CQRS.Hotel.Queries.GetAllHotels
{
    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, Result<List<HotelDto>>>
    {
        private readonly IHotelRepository _hotelRepository;

        public GetAllHotelsQueryHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository ?? throw new ArgumentNullException(nameof(hotelRepository));
        }

        public async Task<Result<List<HotelDto>>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotels = await _hotelRepository.FindAsync(h => request.HotelIds.Count == 0 || request.HotelIds.Contains(h.Id));
                var hotelDtos = new List<HotelDto>();

                foreach (var hotel in hotels)
                {
                    var totalBuildings = await _hotelRepository.CountBuildingsAsync(hotel.Id);

                    hotelDtos.Add(new HotelDto
                    {
                        Id = hotel.Id,
                        Name = hotel.Name,
                        Description = hotel.Description,
                        Address = hotel.Address,
                        Phone = hotel.Phone,
                        Email = hotel.Email,
                        Website = hotel.Website,
                        TotalBuildings = totalBuildings
                    });
                }

                return Result<List<HotelDto>>.Success(hotelDtos);
            }
            catch (Exception ex)
            {
                return Result<List<HotelDto>>.Failure($"Failed to get hotels: {ex.Message}");
            }
        }
    }
}