using HotelBooking.Application.CQRS.Booking.Commands.CheckIn;
using HotelBooking.Application.CQRS.Booking.Commands.CheckOut;
using HotelBooking.Application.CQRS.Booking.Commands.CreateBooking;
using HotelBooking.Application.CQRS.Booking.Commands.UpdateBookingStatus;
using HotelBooking.Application.CQRS.Booking.Queries.GetAllBookings;
using HotelBooking.Application.CQRS.Booking.Queries.GetBookingById;
using HotelBooking.Application.CQRS.Booking.Queries.GetBookingsByCustomer;
using HotelBooking.Application.CQRS.Booking.Queries.GetPendingCheckins;
using HotelBooking.Application.CQRS.Booking.Queries.GetPendingCheckouts;

namespace HotelBooking.API.Controllers
{
    [Authorize(Policy = "emailverified")]
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

        [HttpPost("{id}/checkin")]
        public async Task<IActionResult> CheckIn(int id, [FromBody] CheckInCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/checkout")]
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

        [HttpGet("customer/{guestCitizenIdNumber}")]
        public async Task<IActionResult> GetBookingsByCustomer(string guestCitizenIdNumber)
        {
            var query = new GetBookingsByCustomerQuery { GuestCitizenIdNumber = guestCitizenIdNumber };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings([FromQuery] bool includeInactive = false)
        {
            var query = new GetAllBookingsQuery { IncludeInactive = includeInactive };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("pending-checkins")]
        public async Task<IActionResult> GetPendingCheckins([FromQuery] DateTime date)
        {
            var query = new GetPendingCheckinsQuery { Date = date };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("pending-checkouts")]
        public async Task<IActionResult> GetPendingCheckouts([FromQuery] DateTime date)
        {
            var query = new GetPendingCheckoutsQuery { Date = date };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }
    }
} 