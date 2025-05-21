using HotelBooking.Application.Common.Models;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

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
    }
}
