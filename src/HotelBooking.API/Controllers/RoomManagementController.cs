using HotelBooking.Application.CQRS.Room.Queries;
using HotelBooking.Application.CQRS.RoomManagement.Queries;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    public class RoomManagementController : BaseController
    {
        public RoomManagementController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            var query = new GetRoomTreeQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetRoomDetails(int id)
        {
            var query = new GetRoomByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("room/search")]
        public async Task<IActionResult> SearchRoom([FromQuery] string term)
        {
            var query = new SearchRoomQuery(term);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }
    }
} 