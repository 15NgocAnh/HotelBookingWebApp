using Asp.Versioning;
using HotelBooking.Domain.DTOs.Guest;
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
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuestDTO>> GetGuest(int id)
        {
            var response = await _guestService.GetGuestByIdAsync(id);
            return Ok(response.getData());
        }

        [HttpGet("identity/{identityNumber}")]
        public async Task<ActionResult<GuestDTO>> GetGuestByIdentityNumber(string identityNumber)
        {
            var response = await _guestService.GetGuestByIdentityNumberAsync(identityNumber);
            return Ok(response.getData());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuestDTO>>> GetAllGuests()
        {
            var response = await _guestService.GetAllGuestsAsync();
            return Ok(response.getData());
        }

        [HttpPost]
        public async Task<ActionResult<GuestDTO>> CreateGuest([FromBody] CreateGuestDTO guestDTO)
        {
            var response = await _guestService.CreateGuestAsync(guestDTO);
            return Ok(response.getData());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GuestDTO>> UpdateGuest(int id, [FromBody] CreateGuestDTO guestDTO)
        {
            var response = await _guestService.UpdateGuestAsync(id, guestDTO);
            return Ok(response.getData());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGuest(int id)
        {
            var response = await _guestService.DeleteGuestAsync(id);
            return Ok(response.getMessage());
        }
    }
} 