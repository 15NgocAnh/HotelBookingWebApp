using HotelBooking.Application.CQRS.Amenity.Commands.CreateAmenity;
using HotelBooking.Application.CQRS.Amenity.Commands.DeleteAmentity;
using HotelBooking.Application.CQRS.Amenity.Commands.UpdateAmenity;
using HotelBooking.Application.CQRS.Amenity.Queries.GetAllAmenities;
using HotelBooking.Application.CQRS.Amenity.Queries.GetAmenityById;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    [Authorize(Roles = "SuperAdmin,HotelManager")]
    public class AmenityController : BaseController
    {
        public AmenityController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllAmenitiesQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAmenityByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAmenityCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAmenityCommand command)
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
            var command = new DeleteAmenityCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
