using HotelBooking.Application.CQRS.Booking.DTOs;

namespace HotelBooking.Application.CQRS.Booking.Queries.GetAllBookings
{
    public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, Result<List<BookingDto>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;

        public GetAllBookingsQueryHandler(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IRoomTypeRepository roomTypeRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<BookingDto>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bookings = new List<Domain.AggregateModels.BookingAggregate.Booking>();
                if (request.HotelIds != null && request.HotelIds.Any())
                {
                    bookings = await _bookingRepository.GetBookingsByHotelIdsAsync(request.HotelIds);
                }
                else
                {
                    bookings = (await _bookingRepository.GetAllAsync()).ToList();
                }

                var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);

                foreach (var bookingDto in bookingDtos)
                {
                    var room = await _roomRepository.GetByIdAsync(bookingDto.RoomId);
                    if (room != null)
                    {
                        bookingDto.RoomId = room.Id;
                        bookingDto.RoomName = room.Name;

                        var roomType = await _roomTypeRepository.GetByIdAsync(room.RoomTypeId);

                        if (roomType != null)
                        {
                            bookingDto.RoomTypeName = roomType.Name;
                            bookingDto.RoomPrice = roomType.Price;
                        }
                    }
                }

                return Result<List<BookingDto>>.Success(bookingDtos);
            }
            catch (Exception ex)
            {
                return Result<List<BookingDto>>.Failure($"Failed to get bookings: {ex.Message}");
            }
        }
    }
}