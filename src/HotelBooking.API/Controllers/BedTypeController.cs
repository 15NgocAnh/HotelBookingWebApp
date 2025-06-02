using HotelBooking.Application.CQRS.BedType.Commands.CreateBedType;
using HotelBooking.Application.CQRS.BedType.Commands.DeleteBedType;
using HotelBooking.Application.CQRS.BedType.Commands.UpdateBedType;
using HotelBooking.Application.CQRS.BedType.Queries.GetAllBedTypes;
using HotelBooking.Application.CQRS.BedType.Queries.GetBedTypeById;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    public class BedTypeController : BaseController
    {
        public BedTypeController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllBedTypesQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetBedTypeByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBedTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBedTypeCommand command)
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
            var command = new DeleteBedTypeCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
