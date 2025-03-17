using HotelBooking.Domain.DTOs.Post;
using HotelBooking.Domain.Filtering;
using HotelBooking.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.API.Controllers
{
    [Authorize(Roles = "Admin", Policy = "emailverified")]
    [ApiController]
    [Route("api/v{version:apiVersion}/admin/")]
    public class AdminController : ControllerBase
    {
        private readonly IPostService _postService;

        public AdminController(IPostService postService)
        {
            _postService = postService;
        }

        [Route("post/delete/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePost(int id)
        {
            var serviceResponse = await _postService.DeleteAsync(id);
            return Ok(serviceResponse.getMessage());
        }

        [Route("post/active/{id}")]
        [HttpPut]
        public async Task<ActionResult> ActivePost(int id)
        {
            var serviceResponse = await _postService.ActiveAsync(id);
            return Ok(serviceResponse.getMessage());
        }

        [Route("post/undo/{id}")]
        [HttpPut]
        public async Task<ActionResult> UndoDeletedPost(int id)
        {
            var serviceResponse = await _postService.UndoDeletedAsync(id);
            return Ok(serviceResponse.getMessage());
        }

        [Route("post")]
        [HttpGet]
        public async Task<ActionResult> FilterAllPosts([FromQuery] FilterOptions? filter, [FromQuery] DateDTO? date_filter, string? status_filter)
        {
            var user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var serviceResponse = await _postService.FilterAllAsync(int.Parse(user_id), filter, date_filter, status_filter);
            return Ok(serviceResponse.getData());
        }
    }
}
