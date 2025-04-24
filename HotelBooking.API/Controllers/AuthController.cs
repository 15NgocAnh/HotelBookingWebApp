using Asp.Versioning;
using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/auth/")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
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
            try
            {
                var serviceResponse = await _userServices.RegisterAsync(userdata);
                return CreatedAtAction(nameof(Register), new { version = "1" }, serviceResponse.getMessage());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return BadRequest(new { message = "Registration failed. Please try again." });
            }
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
            try
            {
                var serviceResponse = await _authService.LoginAsync(userdata);
                if (serviceResponse.ResponseType == EResponseType.Success)
                {
                    return Ok(serviceResponse.getData());
                }
                return Unauthorized(new { message = serviceResponse.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return BadRequest(new { message = "Login failed. Please try again." });
            }
        }

        [Route("verify")]
        [Produces("application/json")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Verify()
        {
            try
            {
                var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userid))
                {
                    return BadRequest(new { message = "Invalid user ID" });
                }

                await _authService.verifyEmailAsync(userid);
                return Ok(new { message = "Verification email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during email verification");
                return BadRequest(new { message = "Failed to send verification email" });
            }
        }

        [Route("verify/{token}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult> VerifyLink(string token)
        {
            try
            {
                var decode = WebUtility.UrlDecode(token);
                var serviceResponse = await _authService.activeEmailAsync(decode);
                if (serviceResponse.ResponseType == EResponseType.Success)
                {
                    return Redirect($"https://localhost:7154/Account/Login");
                }
                return BadRequest(new { message = serviceResponse.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during email verification link processing");
                return BadRequest(new { message = "Invalid verification link" });
            }
        }

        [Route("refresh")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> refreshToken(TokenDTO token)
        {
            try
            {
                var serviceResponse = await _authService.refreshTokenAsync(token.Token);
                if (serviceResponse.ResponseType == EResponseType.Success)
                {
                    return CreatedAtAction(nameof(refreshToken), new { version = "1" }, serviceResponse.getData());
                }
                return Unauthorized(new { message = serviceResponse.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return BadRequest(new { message = "Token refresh failed" });
            }
        }

        [Route("forgot")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> forgotPassword(string email)
        {
            try
            {
                var serviceResponse = await _authService.sendForgotEmailVerify(email);
                if (serviceResponse.ResponseType == EResponseType.Success)
                {
                    return Ok(new { message = serviceResponse.Message });
                }
                return NotFound(new { message = serviceResponse.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during forgot password request");
                return BadRequest(new { message = "Failed to process forgot password request" });
            }
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
