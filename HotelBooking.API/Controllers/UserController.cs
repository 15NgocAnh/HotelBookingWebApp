using Asp.Versioning;
using HotelBooking.Domain.DTOs.File;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.API.Controllers
{

    [ApiController]
    [Authorize(Policy = "emailverified")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/user/")]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserServices _userServices;
        public UserController(ILogger<UserController> logger, IUserServices userService)
        {
            _logger = logger;
            _userServices = userService;
        }

        [Route("update-profile")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> updateUserProfile(UserInfoDTO userdata)
        {
            var claims = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            string? userid = claims == null ? null : claims.Value.ToString();
            var serviceResponse = await _userServices.updateUserInfo(userdata, userid);
            return Ok(serviceResponse.getMessage());
        }

        [Route("change-password")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> changePassword(UPasswordDTO passwordDTO)
        {
            var claims = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            string? userid = claims == null ? null : claims.Value.ToString();
            var serviceResponse = await _userServices.changePassword(passwordDTO, userid);
            return Ok(serviceResponse.getMessage());
        }

        [Route("profile")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult> getProfileUser()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userid == null)
            {
                return BadRequest(new ServiceResponse<UserDetailsDTO> { Message = "User Not Found!", ResponseType = EResponseType.BadRequest});
            }
            else
            {
                var serviceResponse = await _userServices.GetUserInfoAsync(int.Parse(userid));
                return Ok(serviceResponse.getData());
            }
        }

        [Route("info/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult> getUserInfoById(int id)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var serviceResponse = await _userServices.GetUserByIdAsync(id, int.Parse(userid));
            return Ok(serviceResponse);
        }

        [Route("upload-avatar")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> uploadAvatar(FileDTO file)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            //var serviceResponse = _s3Services.PresignedUpload(file.file_name, file.file_type, CJConstant.AVATAR_PATH, userid);
            await _userServices.updateAvatar(file, userid);
            return Ok();
        }
    }
}