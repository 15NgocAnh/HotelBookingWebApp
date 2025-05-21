using HotelBooking.Application.CQRS.Auth.Commands.RefreshToken;
using HotelBooking.Application.CQRS.Auth.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace HotelBooking.Web.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task<bool> RefreshTokenAsync();
    }

    public class TokenService : ITokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TokenService> _logger;

        public TokenService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TokenService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public Task<string> GetTokenAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context?.Items["AccessToken"] is string cachedToken)
                return Task.FromResult(cachedToken);

            var token = context?.Request.Cookies["JWT"];
            return Task.FromResult(token ?? string.Empty);
        }

        public async Task<bool> RefreshTokenAsync()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["JWT"];
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"]; 
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            try
            {
                var client = _httpClientFactory.CreateClient("BackendApi");
                var response = await client.PostAsJsonAsync("api/auth/refresh-token", new RefreshTokenCommand() { AccessToken = accessToken, RefreshToken = refreshToken });
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Token refresh failed: {Content}", content);
                    return false;
                }

                var jsonDoc = JsonDocument.Parse(content);
                var root = jsonDoc.RootElement;
                var newAccessToken = root.GetProperty("token").GetString();

                if (string.IsNullOrWhiteSpace(newAccessToken))
                {
                    _logger.LogWarning("New access token not found in refresh response.");
                    return false;
                }

                // Store the token temporarily in HttpContext for reuse
                _httpContextAccessor.HttpContext!.Items["AccessToken"] = newAccessToken;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return false;
            }
        }
    }
} 