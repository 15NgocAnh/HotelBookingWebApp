using HotelBooking.Application.CQRS.Auth.Commands.RefreshToken;
using HotelBooking.Infrastructure.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace HotelBooking.API.Middleware
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMediator _mediator;

        public RefreshTokenMiddleware(RequestDelegate next, IMediator mediator)
        {
            _next = next;
            _mediator = mediator;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SecurityTokenExpiredException)
            {
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (!string.IsNullOrEmpty(token) && JWTHelper.JwtExpired(token))
                {
                    var refreshToken = context.Request.Headers["Refresh-Token"].ToString();

                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        var command = new RefreshTokenCommand
                        {
                            AccessToken = token,
                            RefreshToken = refreshToken
                        };

                        var result = await _mediator.Send(command);
                        if (result.IsSuccess)
                        {
                            context.Response.Headers.Add("New-Access-Token", result.Data.Token);
                            context.Response.Headers.Add("New-Refresh-Token", result.Data.RefreshToken);
                            context.Response.Headers.Add("Access-Control-Expose-Headers", "New-Access-Token,New-Refresh-Token");
                        }
                    }
                }
            }
            catch
            {
                throw; // các lỗi khác giữ nguyên
            }
        }
    }
} 