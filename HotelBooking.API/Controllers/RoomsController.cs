using Asp.Versioning;
using HotelBooking.Application.Features.Rooms.Commands;
using HotelBooking.Application.Features.Rooms.Queries;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "emailverified")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]/")]
    public class RoomsController(IMediator mediator, IRoomService roomService) : BaseController(mediator)
    {
        [HttpGet]
        public async Task<IActionResult> GetRooms([FromQuery] GetRoomsQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var serviceResponse = await roomService.GetByIdAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpGet("floor/{floorId}")]
        public async Task<ActionResult> GetByFloorId(int floorId)
        {
            var serviceResponse = await roomService.GetByFloorIdAsync(floorId);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpGet("type/{roomTypeId}")]
        public async Task<ActionResult> GetByRoomTypeId(int roomTypeId)
        {
            var serviceResponse = await roomService.GetByRoomTypeIdAsync(roomTypeId);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateRoomDTO updateRoomDTO)
        {
            var serviceResponse = await roomService.UpdateAsync(id, updateRoomDTO);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());
            if (serviceResponse.ResponseType == EResponseType.BadRequest)
                return BadRequest(serviceResponse.getMessage());
            if (serviceResponse.ResponseType == EResponseType.InternalError)
                return BadRequest(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var serviceResponse = await roomService.DeleteAsync(id);
            if (serviceResponse.ResponseType == EResponseType.NotFound)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getMessage());
        }
    }
}
