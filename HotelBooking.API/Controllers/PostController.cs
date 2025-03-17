using Asp.Versioning;
using HotelBooking.Domain.DTOs.Common;
using HotelBooking.Domain.DTOs.Post;
using HotelBooking.Domain.Filtering;
using HotelBooking.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "emailverified")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/post/")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult> getAllPosts([FromQuery] FilterOptions? filter)
        {
            var serviceResponse = await _postService.GetAllAsync(filter);
            return Ok(serviceResponse.getData());
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> getPostById(int id)
        {
            var serviceResponse = await _postService.FindByIdAsync(id);
            return Ok(serviceResponse.getData());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> updatePost(int id, PostDTO post)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var serviceResponse = await _postService.UpdateAsync(int.Parse(userid), id, post);
            return Ok(serviceResponse.getMessage());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deletePost(int id)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var serviceResponse = await _postService.DeleteAsync(int.Parse(userid), id);
            return Ok(serviceResponse.getMessage());
        }
    }
}
