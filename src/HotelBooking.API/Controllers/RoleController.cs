using HotelBooking.Application.CQRS.Role.Commands.CreateRole;
using HotelBooking.Application.CQRS.Role.Commands.DeleteRole;
using HotelBooking.Application.CQRS.Role.Commands.UpdateRole;
using HotelBooking.Application.CQRS.Role.Queries.GetAllRoles;
using HotelBooking.Application.CQRS.Role.Queries.GetRoleById;

namespace HotelBooking.API.Controllers
{
    public class RoleController : BaseController
    {
        public RoleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteRoleCommand { Id = id };
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetRoleByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllRolesQuery query)
        {
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }
    }
}