using Asp.Versioning;
using HotelBooking.Application.CQRS.Booking.Commands;
using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.CQRS.Booking.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "emailverified")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
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