using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Domain.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IBookingRoomRepository _bookingRoomRepository;
        private readonly IMapper _mapper;

        public BookingService(
            IBookingRepository bookingRepository, 
            IGuestRepository guestRepository, 
            IBookingRoomRepository bookingRoomRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _guestRepository = guestRepository;
            _bookingRoomRepository = bookingRoomRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<BookingDTO>> CreateBookingAsync(CreateBookingDTO bookingDTO)
        {
            var serviceResponse = new ServiceResponse<BookingDTO>();
            try
            {
                // Check if guest exists by identity number
                var guest = await _guestRepository.GetByIdentityNumberAsync(bookingDTO.Guest.IdentityNumber);
                if (guest == null)
                {
                    // Create new guest
                    var newGuest = new GuestModel
                    {
                        FullName = bookingDTO.Guest?.FullName,
                        IdentityNumber = bookingDTO.Guest?.IdentityNumber,
                        IdentityIssueDate = bookingDTO.Guest?.IdentityIssueDate,
                        IdentityIssuePlace = bookingDTO.Guest?.IdentityNumber,
                        Address = bookingDTO.Guest?.IdentityNumber,
                        Nationality = bookingDTO.Guest?.IdentityNumber,
                        Gender = bookingDTO.Guest?.Gender ?? Gender.Other,
                        BirthDate = bookingDTO.Guest?.BirthDate,
                        Province = bookingDTO.Guest?.Province,
                        Phone = bookingDTO.Guest?.Phone,
                        Email = bookingDTO.Guest?.Email
                    };
                    guest = await _guestRepository.AddAsync(newGuest);
                }

                // Create booking
                var booking = new BookingModel
                {
                    GuestId = guest != null ? guest.Id : 1,
                    EstCheckinTime = bookingDTO.EstCheckinTime,
                    EstCheckoutTime = bookingDTO.EstCheckoutTime,
                    NumberOfAdults = bookingDTO.NumberOfAdults,
                    NumberOfChildren = bookingDTO.NumberOfChildren,
                    BookingType = bookingDTO.BookingType,
                    Note = bookingDTO.Note,
                    Guest = guest,
                };

                booking = await _bookingRepository.AddAsync(booking);
                
                if (bookingDTO.Rooms != null && bookingDTO.Rooms.Any())
                {
                    foreach (var roomDto in bookingDTO.Rooms)
                    {
                        var bookingRoom = new BookingRoom
                        {
                            BookingId = booking.Id,
                            RoomId = roomDto.Id
                        };
                        await _bookingRoomRepository.AddAsync(bookingRoom);
                    }
                }

                serviceResponse.Data = _mapper.Map<BookingDTO>(booking);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Create booking successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<BookingDTO>> GetBookingByIdAsync(int bookingId)
        {
            var serviceResponse = new ServiceResponse<BookingDTO>();
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Booking not found";
                    return serviceResponse;
                }

                serviceResponse.Data = _mapper.Map<BookingDTO>(booking);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get booking successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<BookingDTO>>> GetBookingsByGuestIdAsync(int guestId)
        {
            var serviceResponse = new ServiceResponse<List<BookingDTO>>();
            try
            {
                var bookings = await _bookingRepository.GetAllAsync();
                var guestBookings = bookings.Where(b => b.GuestId == guestId).ToList();
                
                if (!guestBookings.Any())
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "No bookings found for this guest";
                    return serviceResponse;
                }

                serviceResponse.Data = _mapper.Map<List<BookingDTO>>(guestBookings);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get guest bookings successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> CancelBookingAsync(int bookingId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Booking not found";
                    return serviceResponse;
                }

                booking.Status = BookingStatus.Cancelled;
                await _bookingRepository.UpdateAsync(booking);
                
                serviceResponse.Data = true;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Cancel booking successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateBookingStatusAsync(int bookingId, BookingStatus status)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Booking not found";
                    return serviceResponse;
                }

                booking.Status = status;
                await _bookingRepository.UpdateAsync(booking);
                
                serviceResponse.Data = true;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Update booking status successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> CheckInAsync(int bookingId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Booking not found";
                    return serviceResponse;
                }

                // Check if booking is in valid state for check-in
                if (booking.Status != BookingStatus.Confirmed)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Booking must be in Confirmed state to check in";
                    return serviceResponse;
                }

                // Check if current date is within check-in date range
                var currentDate = DateTime.Now.Date;
                if (currentDate < booking.EstCheckinTime.Date || currentDate > booking.EstCheckoutTime.Date)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Cannot check in outside of booking date range";
                    return serviceResponse;
                }

                booking.Status = BookingStatus.CheckedIn;
                booking.ActualCheckInTime = DateTime.Now;
                await _bookingRepository.UpdateAsync(booking);

                serviceResponse.Data = true;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Check in successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> CheckOutAsync(int bookingId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Booking not found";
                    return serviceResponse;
                }

                // Check if booking is in valid state for check-out
                if (booking.Status != BookingStatus.CheckedIn)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Booking must be in CheckedIn state to check out";
                    return serviceResponse;
                }

                // Check if current date is within check-out date range
                var currentDate = DateTime.Now.Date;
                if (currentDate < booking.EstCheckinTime.Date || currentDate > booking.EstCheckoutTime.Date)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Cannot check out outside of booking date range";
                    return serviceResponse;
                }

                booking.Status = BookingStatus.CheckedOut;
                booking.ActualCheckOutTime = DateTime.Now;
                await _bookingRepository.UpdateAsync(booking);

                serviceResponse.Data = true;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Check out successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
} 