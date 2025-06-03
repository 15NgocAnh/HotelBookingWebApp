using HotelBooking.Application.CQRS.ExtraCategory.Commands;
using HotelBooking.Application.CQRS.ExtraCategory.Queries;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    public class ExtraCategoryController : BaseController
    {
        public ExtraCategoryController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllExtraCategoriesQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetExtraCategoryByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> Create([FromBody] CreateExtraCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExtraCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match ID in request body");
            }

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteExtraCategoryCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
