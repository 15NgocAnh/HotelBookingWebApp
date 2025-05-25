using HotelBooking.Application.CQRS.Booking.DTOs;

namespace HotelBooking.Application.CQRS.Booking.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;

        public GetBookingByIdQueryHandler(
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

        public async Task<Result<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(request.Id);
                if (booking == null)
                    return Result<BookingDto>.Failure("Booking not found");

                var bookingDto = _mapper.Map<BookingDto>(booking);

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

                return Result<BookingDto>.Success(bookingDto);
            }
            catch (Exception ex)
            {
                return Result<BookingDto>.Failure($"Error retrieving booking: {ex.Message}");
            }
        }
    }
}