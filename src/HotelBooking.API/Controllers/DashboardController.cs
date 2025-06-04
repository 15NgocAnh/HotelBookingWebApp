using HotelBooking.Application.CQRS.Dashboard.Queries;

namespace HotelBooking.API.Controllers
{
    [Authorize(Roles = "SuperAdmin,HotelManager")]
    public class DashboardController : BaseController
    {
        public DashboardController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var query = new GetDashboardStatisticsQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }
    }
} 