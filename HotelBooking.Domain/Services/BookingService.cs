using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.Repository.Interfaces;
using HotelBooking.Domain.Services.Interfaces;

namespace HotelBooking.Domain.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<BookingDTO> CreateBookingAsync(BookingDTO bookingDTO)
        {
            // Validate room availability
            var existingBookings = await _bookingRepository.GetAllAsync();
            var isRoomAvailable = !existingBookings.Any(b => 
                b.RoomId == bookingDTO.RoomId && 
                b.Status != BookingStatus.Cancelled &&
                ((bookingDTO.ArrivalDate >= b.ArrivalDate && bookingDTO.ArrivalDate < b.DepartureDate) ||
                 (bookingDTO.DepartureDate > b.ArrivalDate && bookingDTO.DepartureDate <= b.DepartureDate) ||
                 (bookingDTO.ArrivalDate <= b.ArrivalDate && bookingDTO.DepartureDate >= b.DepartureDate)));

            if (!isRoomAvailable)
            {
                throw new InvalidOperationException("Room is not available for the selected dates");
            }

            var booking = _mapper.Map<BookingModel>(bookingDTO);
            booking.BookingDateTime = DateTime.Now;
            booking.Status = BookingStatus.Pending;

            await _bookingRepository.AddAsync(booking);
            return bookingDTO;
        }

        public async Task<BookingDTO> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task<IEnumerable<BookingDTO>> GetBookingsByGuestIdAsync(int guestId)
        {
            var bookings = await _bookingRepository.GetAllAsync();
            var guestBookings = bookings.Where(b => b.GuestId == guestId);
            return _mapper.Map<IEnumerable<BookingDTO>>(guestBookings);
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                return false;

            booking.Status = BookingStatus.Cancelled;
            await _bookingRepository.UpdateAsync(booking);
            return true;
        }

        public async Task<bool> UpdateBookingStatusAsync(int bookingId, BookingStatus status)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                return false;

            booking.Status = status;
            await _bookingRepository.UpdateAsync(booking);
            return true;
        }
    }
} 