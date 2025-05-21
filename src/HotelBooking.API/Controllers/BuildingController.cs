using HotelBooking.Application.CQRS.Building.Commands;
using HotelBooking.Application.CQRS.Building.Queries;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    public class BuildingController : BaseController
    {
        public BuildingController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllBuildingsQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("floor/{buildingId}")]
        public async Task<IActionResult> GetAllFloorsByBuildingId(int buildingId)
        {
            var query = new GetAllFloorsByBuildingIdQuery(buildingId);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetBuildingByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var query = new GetBuildingsByHotelQuery(hotelId);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBuildingCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBuildingCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match ID in request body");
            }

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteBuildingCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
