using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Queries.GetBookingsByCustomer
{
    public class GetBookingsByCustomerQueryHandler : IRequestHandler<GetBookingsByCustomerQuery, Result<List<BookingDto>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingsByCustomerQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<BookingDto>>> Handle(GetBookingsByCustomerQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bookings = await _bookingRepository.GetBookingsByCustomerIdAsync(request.GuestCitizenIdNumber);
                var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);
                return Result<List<BookingDto>>.Success(bookingDtos);
            }
            catch (Exception ex)
            {
                return Result<List<BookingDto>>.Failure($"Error retrieving customer bookings: {ex.Message}");
            }
        }
    }
}