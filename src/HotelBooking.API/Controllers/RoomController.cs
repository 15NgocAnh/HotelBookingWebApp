using HotelBooking.Application.CQRS.Room.Commands;
using HotelBooking.Application.CQRS.Room.Queries.GetAllRoomAvailable;
using HotelBooking.Application.CQRS.Room.Queries.GetAllRooms;
using HotelBooking.Application.CQRS.Room.Queries.GetRoomById;
using HotelBooking.Application.CQRS.Room.Queries.GetRoomsByBuilding;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    public class RoomController : BaseController
    {
        public RoomController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllRoomsQuery();
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

        [HttpGet("available")]
        public async Task<IActionResult> GetAllAvailable()
        {
            var query = new GetAllRoomsAvailableQuery();
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetRoomByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("building/{buildingId}")]
        public async Task<IActionResult> GetByBuilding(int buildingId)
        {
            var query = new GetRoomsByBuildingQuery(buildingId);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> Create(CreateRoomCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> Update(int id, UpdateRoomCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteRoomCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeRoomStatusCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
} 