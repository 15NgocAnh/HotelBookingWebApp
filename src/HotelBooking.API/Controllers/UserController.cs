using HotelBooking.Application.CQRS.Hotel.Queries.GetAllHotels;
using HotelBooking.Application.CQRS.User.Commands.ChangePassword;
using HotelBooking.Application.CQRS.User.Commands.CreateUser;
using HotelBooking.Application.CQRS.User.Commands.DeleteUser;
using HotelBooking.Application.CQRS.User.Commands.UpdateProfile;
using HotelBooking.Application.CQRS.User.Commands.UpdateUser;
using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Application.CQRS.User.Queries.GetAllUsers;
using HotelBooking.Application.CQRS.User.Queries.GetUserById;

namespace HotelBooking.API.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUserCommand { Id = id };
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}/hotels")]
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> GetAllHotelsByUser(int id)
        {
            var query = new GetAllHotelsByUserQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto profile)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest("Unauthoried");
            }
            var command = new UpdateProfileCommand
            {
                UserId = int.Parse(userId),
                Profile = profile
            };

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto password)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest("Unauthoried");
            }
            var command = new ChangePasswordCommand
            {
                UserId = int.Parse(userId),
                Password = password
            };

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
} 