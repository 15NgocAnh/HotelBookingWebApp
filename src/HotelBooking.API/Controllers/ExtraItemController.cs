using HotelBooking.Application.CQRS.ExtraCategory.Queries;
using HotelBooking.Application.CQRS.ExtraItem.Commands;
using HotelBooking.Application.CQRS.ExtraItem.Queries;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    public class ExtraItemController : BaseController
    {
        public ExtraItemController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllExtraItemsQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetExtraItemByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var query = new GetExtraItemsByCategoryQuery(categoryId);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExtraItemCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExtraItemCommand command)
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
            var command = new DeleteExtraItemCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
