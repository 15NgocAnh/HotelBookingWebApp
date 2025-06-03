using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.Commands.CheckIn;
using HotelBooking.Application.CQRS.Booking.Commands.CheckOut;
using HotelBooking.Application.CQRS.Booking.Commands.CreateBooking;
using HotelBooking.Application.CQRS.Booking.Commands.UpdateBooking;
using HotelBooking.Application.CQRS.Booking.Commands.UpdateBookingStatus;
using HotelBooking.Application.CQRS.Booking.Queries.GetAllBookings;
using HotelBooking.Application.CQRS.Booking.Queries.GetBookingById;
using HotelBooking.Application.CQRS.Booking.Queries.GetPendingCheckins;
using HotelBooking.Application.CQRS.Booking.Queries.GetPendingCheckouts;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    public class BookingController : BaseController
    {
        public BookingController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] UpdateBookingStatusCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}/checkin")]
        public async Task<IActionResult> CheckIn(int id, [FromBody] CheckInCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}/checkout")]
        public async Task<IActionResult> CheckOut(int id, [FromBody] CheckOutCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var query = new GetBookingByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings([FromQuery] bool includeInactive = false)
        {
            var query = new GetAllBookingsQuery { IncludeInactive = includeInactive };
            if (!IsSuperAdmin)
            {
                var hotelIds = GetUserHotelIds();
                if (hotelIds == null || hotelIds.Count == 0)
                {
                    return Unauthorized("User không có hotelId hợp lệ.");
                }
                query.HotelIds = hotelIds;
            }

            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("pending-checkins")]
        public async Task<IActionResult> GetPendingCheckins([FromQuery] DateTime date)
        {
            var query = new GetPendingCheckinsQuery { Date = date };
            if (!IsSuperAdmin)
            {
                var hotelIds = GetUserHotelIds();
                if (hotelIds == null || hotelIds.Count == 0)
                {
                    return Unauthorized("User không có hotelId hợp lệ.");
                }
                query.HotelIds = hotelIds;
            }

            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("pending-checkouts")]
        public async Task<IActionResult> GetPendingCheckouts([FromQuery] DateTime date)
        {
            var query = new GetPendingCheckoutsQuery { Date = date };
            if (!IsSuperAdmin)
            {
                var hotelIds = GetUserHotelIds();
                if (hotelIds == null || hotelIds.Count == 0)
                {
                    return Unauthorized("User không có hotelId hợp lệ.");
                }
                query.HotelIds = hotelIds;
            }

            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(Result.Failure("Invalid booking ID"));
            }

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
} 