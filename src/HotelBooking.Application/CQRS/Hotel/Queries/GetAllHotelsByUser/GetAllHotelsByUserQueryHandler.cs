using HotelBooking.Application.CQRS.Hotel.DTOs;

namespace HotelBooking.Application.CQRS.Hotel.Queries.GetAllHotels
{
    public class GetAllHotelsByUserQueryHandler : IRequestHandler<GetAllHotelsByUserQuery, Result<List<HotelDto>>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserHotelRepository _userHotelRepository;

        public GetAllHotelsByUserQueryHandler(
            IHotelRepository hotelRepository,
            IUserHotelRepository userHotelRepository)
        {
            _hotelRepository = hotelRepository ?? throw new ArgumentNullException(nameof(hotelRepository));
            _userHotelRepository = userHotelRepository;
        }

        public async Task<Result<List<HotelDto>>> Handle(GetAllHotelsByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userHotels = await _userHotelRepository.FindAsync(u => u.UserId == request.id);
                if (userHotels == null)
                {
                    return Result<List<HotelDto>>.Failure("Not found hotels by user");
                }
                var hotelIds = userHotels.Select(uh => uh.HotelId).ToList();
                var hotels = await _hotelRepository.FindAsync(h => hotelIds.Contains(h.Id));
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