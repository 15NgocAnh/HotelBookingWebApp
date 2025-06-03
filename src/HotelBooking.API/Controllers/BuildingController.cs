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
        [Authorize(Roles = "SuperAdmin,HotelManager")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllBuildingsQuery();
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
            if (!CheckHotelAccess(id))
            {
                return Unauthorized("User không có quyền truy cập building này.");
            }

            var query = new GetBuildingByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            if (!CheckHotelAccess(hotelId))
            {
                return Unauthorized("User không có quyền truy cập hotel này.");
            }

            var query = new GetBuildingsByHotelQuery(hotelId);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateBuildingCommand command)
        {
            if (!IsSuperAdmin)
            {
                return Unauthorized("User không có quyền tạo building cho hotel này.");
            }

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBuildingCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match ID in request body");
            }

            if (!IsSuperAdmin)
            {
                return Unauthorized("User không có quyền cập nhật building này.");
            }

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsSuperAdmin)
            {
                return Unauthorized("User không có quyền xóa building này.");
            }

            var command = new DeleteBuildingCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
