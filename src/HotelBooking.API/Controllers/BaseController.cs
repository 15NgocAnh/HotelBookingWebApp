using HotelBooking.Application.Common.Models;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        protected bool IsSuperAdmin => User.IsInRole("SuperAdmin");

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            
            if (result.IsSuccess && result.Data != null)
                return Ok(result.Data);
                
            if (result.IsSuccess && result.Data == null)
                return NotFound();
                
            return BadRequest(result.Messages);
        }

        protected IActionResult HandleResult(Result result)
        {
            if (result == null) return NotFound();
            
            if (result.IsSuccess)
                return Ok();
                
            return BadRequest(result.Messages);
        }

        protected bool CheckHotelAccess(int hotelId)
        {
            // SuperAdmin => có quyền truy cập tất cả hotels
            if (User.IsInRole("SuperAdmin"))
                return true;

            // Lấy danh sách hotelId từ claims
            var hotelIds = User.FindAll("HotelId")
                .Select(c => int.TryParse(c.Value, out int id) ? id : 0)
                .Where(id => id > 0)
                .ToList();

            // Kiểm tra hotelId có trong danh sách
            return hotelIds.Contains(hotelId);
        }

        protected List<int>? GetUserHotelIds()
        {
            // Lấy danh sách hotelId từ claims
            return User.FindAll("HotelId")
                .Select(c => int.TryParse(c.Value, out int id) ? id : 0)
                .Where(id => id > 0)
                .ToList();
        }
    }
}
