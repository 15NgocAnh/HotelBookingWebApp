using HotelBooking.Application.CQRS.Hotel.Commands.CreateHotel;
using HotelBooking.Application.CQRS.Hotel.Commands.DeleteHotel;
using HotelBooking.Application.CQRS.Hotel.Commands.UpdateHotel;
using HotelBooking.Application.CQRS.Hotel.Queries.GetAllHotels;
using HotelBooking.Application.CQRS.Hotel.Queries.GetHotelById;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    public class HotelController : BaseController
    {
        public HotelController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllHotelsQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetHotelByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateHotelCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHotelCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match ID in request body");
            }

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteHotelCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
