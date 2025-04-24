using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDTO>> CreateBooking([FromBody] BookingDTO bookingDTO)
        {
            try
            {
                var result = await _bookingService.CreateBookingAsync(bookingDTO);
                return Ok(new { data = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while creating the booking" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking == null)
                    return NotFound(new { error = "Booking not found" });

                return Ok(new { data = booking });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the booking" });
            }
        }

        [HttpGet("guest/{guestId}")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetGuestBookings(int guestId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByGuestIdAsync(guestId);
                return Ok(new { data = bookings });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the bookings" });
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> CancelBooking(int id)
        {
            try
            {
                var result = await _bookingService.CancelBookingAsync(id);
                if (!result)
                    return NotFound(new { error = "Booking not found" });

                return Ok(new { message = "Booking cancelled successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while cancelling the booking" });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatus status)
        {
            try
            {
                var result = await _bookingService.UpdateBookingStatusAsync(id, status);
                if (!result)
                    return NotFound(new { error = "Booking not found" });

                return Ok(new { message = "Booking status updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while updating the booking status" });
            }
        }
    }
} 