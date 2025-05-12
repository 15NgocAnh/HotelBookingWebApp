using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Queries
{
    public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, Result<List<BookingDto>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetAllBookingsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<BookingDto>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bookings = request.IncludeInactive
                    ? await _bookingRepository.GetAllAsync()
                    : await _bookingRepository.GetActiveBookingsAsync();

                var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);
                return Result<List<BookingDto>>.Success(bookingDtos);
            }
            catch (System.Exception ex)
            {
                return Result<List<BookingDto>>.Failure($"Error retrieving bookings: {ex.Message}");
            }
        }
    }
} 