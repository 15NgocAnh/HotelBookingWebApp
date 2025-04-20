using Asp.Versioning;
using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace HotelBooking.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/auth/")]
    public class AuthController : Controller
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly IUserServices _userServices;
        private readonly IAuthenticationServices _authService;
        public AuthController(ILogger<AuthController> logger, IUserServices userService, IAuthenticationServices authService)
        {
            _logger = logger;
            _userServices = userService;
            _authService = authService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userdata"></param>
        /// <response code="201">User registration successful!</response>
        /// <exception cref="NotImplementedException"></exception>
        [Route("register")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserRegisterDTO userdata)
        {
            var serviceResponse = await _userServices.RegisterAsync(userdata);
            return CreatedAtAction(nameof(Register), new { version = "1" }, serviceResponse.getMessage());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userdata"></param>
        /// <returns></returns>
        [Route("login")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO userdata)
        {
            var serviceResponse = await _authService.LoginAsync(userdata);
            return Ok(serviceResponse.getData());
        }

        [Route("verify")]
        [Produces("application/json")]
        [HttpPost]
        [Authorize]
        public async Task Verify()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _authService.verifyEmailAsync(userid);
        }

        [Route("verify/{token}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult> VerifyLink(string token)
        {
            var decode = WebUtility.UrlDecode(token);
            var serviceResponse = await _authService.activeEmailAsync(decode);
            return Redirect($"https://localhost:7154/Account/Login");
        }

        [Route("refresh")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> refreshToken(TokenDTO token)
        {
            var serviceResponse = await _authService.refreshTokenAsync(token.Token);
            return CreatedAtAction(nameof(refreshToken), new { version = "1" }, serviceResponse.getData());
        }

        [Route("forgot")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> forgotPassword(string email)
        {
            var serviceResponse = await _authService.sendForgotEmailVerify(email);
            return Ok(serviceResponse.getMessage());
        }

        [Route("forgot/{token}")]
        [Produces("application/json")]
        [HttpGet]
        public IActionResult ForgotPasswordRedirect(string token)
        {
            // Giải mã token (nếu cần)
            var decodedToken = WebUtility.UrlDecode(token);

            // Chuyển hướng sang trang ResetPassword.cshtml với token trên URL
            return Redirect($"https://localhost:7154/Account/ResetPassword?token={decodedToken}");
        }

        [Route("reset-password")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO request)
        {
            var serviceResponse = await _authService.resetPasswordAsync(request.Token, request.NewPassword);
            return Ok(serviceResponse.getMessage());
        }

        [Route("logout")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> logout(TokenDTO token)
        {
            var serviceResponse = await _authService.logout(token.Token);
            return Ok(serviceResponse.getMessage());
        }
    }
}
