using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Queries
{
    public class GetPendingCheckinsQueryHandler : IRequestHandler<GetPendingCheckinsQuery, Result<List<BookingDto>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetPendingCheckinsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<BookingDto>>> Handle(GetPendingCheckinsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bookings = await _bookingRepository.GetPendingCheckinsAsync(request.Date);
                var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);
                return Result<List<BookingDto>>.Success(bookingDtos);
            }
            catch (System.Exception ex)
            {
                return Result<List<BookingDto>>.Failure($"Error retrieving pending check-ins: {ex.Message}");
            }
        }
    }
} 