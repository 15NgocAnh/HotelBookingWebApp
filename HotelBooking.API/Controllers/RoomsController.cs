using Asp.Versioning;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "emailverified")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/room/")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        public RoomsController(IRoomService roomService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRooms()
        {
            var serviceResponse = await _roomService.GetAllAsync();
            return Ok(serviceResponse.getData());
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchRooms([FromQuery] RoomCondition condition)
        {
            var serviceResponse = await _roomService.SearchRoomsAsync(condition);
            return Ok(serviceResponse.getData());
        }

        [HttpGet("roomtypes")]
        public async Task<ActionResult> GetAllRoomTypes()
        {
            var serviceResponse = await _roomTypeService.GetAllAsync();
            return Ok(serviceResponse.getData());
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> GetRoomById(int id)
        {
            var serviceResponse = await _roomService.FindByIdAsync(id);
            return Ok(serviceResponse.getData());
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomDetailsDTO room)
        {
            var serviceResponse = await _roomService.SaveAsync(room);
            return Ok(serviceResponse.getMessage());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoom(int id, [FromBody] RoomDetailsDTO room)
        {
            var serviceResponse = await _roomService.UpdateAsync(id, room);
            return Ok(serviceResponse.getData());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            var serviceResponse = await _roomService.DeleteAsync(id);
            return Ok(serviceResponse.getMessage());
        }
    }
}
