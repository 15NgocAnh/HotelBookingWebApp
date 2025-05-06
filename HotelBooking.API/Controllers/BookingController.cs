using Asp.Versioning;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "emailverified")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDTO>> CreateBooking([FromBody] CreateBookingDTO bookingDTO)
        {
            var response = await _bookingService.CreateBookingAsync(bookingDTO);
            if (response.ResponseType == EResponseType.Success)
            {
                return Ok(response.getData());
            }
            return StatusCode((int)response.ResponseType, response.getMessage());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            var response = await _bookingService.GetBookingByIdAsync(id);
            if (response.ResponseType == EResponseType.Success)
            {
                return Ok(response.getData());
            }
            return StatusCode((int)response.ResponseType, response.getMessage());
        }

        [HttpGet("guest/{guestId}")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetGuestBookings(int guestId)
        {
            var response = await _bookingService.GetBookingsByGuestIdAsync(guestId);
            if (response.ResponseType == EResponseType.Success)
            {
                return Ok(response.getData());
            }
            return StatusCode((int)response.ResponseType, response.getMessage());
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> CancelBooking(int id)
        {
            var response = await _bookingService.CancelBookingAsync(id);
            if (response.ResponseType == EResponseType.Success)
            {
                return Ok(response.getMessage());
            }
            return StatusCode((int)response.ResponseType, response.getMessage());
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatus status)
        {
            var response = await _bookingService.UpdateBookingStatusAsync(id, status);
            if (response.ResponseType == EResponseType.Success)
            {
                return Ok(response.getMessage());
            }
            return StatusCode((int)response.ResponseType, response.getMessage());
        }

        [HttpPost("{id}/checkin")]
        public async Task<ActionResult> CheckIn(int id)
        {
            var response = await _bookingService.CheckInAsync(id);
            if (response.ResponseType == EResponseType.Success)
            {
                return Ok(response.getMessage());
            }
            return StatusCode((int)response.ResponseType, response.getMessage());
        }

        [HttpPost("{id}/checkout")]
        public async Task<ActionResult> CheckOut(int id)
        {
            var response = await _bookingService.CheckOutAsync(id);
            if (response.ResponseType == EResponseType.Success)
            {
                return Ok(response.getMessage());
            }
            return StatusCode((int)response.ResponseType, response.getMessage());
        }
    }
} 