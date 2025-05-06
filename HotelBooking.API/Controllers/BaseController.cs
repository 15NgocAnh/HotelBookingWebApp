using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    public class BaseController(IMediator mediator) : ControllerBase
    {
    }
}
