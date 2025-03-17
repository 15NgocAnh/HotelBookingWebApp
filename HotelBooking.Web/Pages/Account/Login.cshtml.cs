using HotelBooking.Domain.DTOs.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HotelBooking.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        [BindProperty]
        public LoginInput Input { get; set; }

        public string? ReturnUrl { get; set; }
        public string? ErrorMessage { get; set; }

        public LoginModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("BackendApi");
            _httpClient.BaseAddress = new Uri(_configuration["BackendApi"]);
        }

        public IActionResult OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                return await Login(returnUrl);
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred during login. Please try again.";
                return Page();
            }
        }
        private async Task<IActionResult> Login(string returnUrl)
        {
            var credential = await GetJWT();
            if (credential == null || string.IsNullOrEmpty(credential.Data.Token))
            {
                ErrorMessage = "Invalid username or password";
                return Page();
            }

            var userInfo = GetUserInfoFromJWT(credential.Data.Token);
            if (string.IsNullOrEmpty(userInfo.Id))
            {
                ErrorMessage = "Failed to retrieve user information.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
                new Claim(ClaimTypes.Name, userInfo.Username),
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim("UserInfo", JsonSerializer.Serialize(userInfo))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration["Session:TimeOut"] ?? "30")),
                IsPersistent = false,
                IssuedUtc = DateTime.UtcNow,
                RedirectUri = returnUrl
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(returnUrl);
        }

        private async Task<ApiResponse<CredentialDTO>?> GetJWT()
        {
            try
            {
                Input.SessionId = Guid.NewGuid().ToString();
                Input.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                var content = new StringContent(JsonSerializer.Serialize(Input), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/v1/auth/login", content);
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseText, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (errorResponse?.Errors != null)
                        {
                            foreach (var error in errorResponse.Errors)
                            {
                                ModelState.AddModelError(string.Empty, string.Join(", ", error.Value));
                            }
                        }
                    }

                    return null;
                }

                var credential = JsonSerializer.Deserialize<ApiResponse<CredentialDTO>>(responseText, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (credential != null)
                {
                    HttpContext.Response.Cookies.Append("JWT", credential.Data.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration["Session:TimeOut"] ?? "30"))
                    });

                    HttpContext.Response.Cookies.Append("RefreshToken", credential.Data.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(7) // Refresh Token thường có thời gian dài hơn
                    });
                }

                return credential;
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Lỗi hệ thống, vui lòng thử lại.");
                return null;
            }
        }

        public class ApiResponse<T>
        {
            [JsonPropertyName("data")]
            public T Data { get; set; }
        }


        public class ErrorResponse
        {
            public Dictionary<string, List<string>> Errors { get; set; } = new();
        }

        private UserInfo GetUserInfoFromJWT(string jwt)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwt) as JwtSecurityToken;

                if (jsonToken == null)
                {
                    return new UserInfo();
                }

                return new UserInfo
                {
                    Id = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "",
                    Username = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "",
                    Email = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "",
                    IsActive = true
                };
            }
            catch
            {
                return new UserInfo();
            }
        }


        // This method is not needed in the login flow - should be moved to a shared service
        public async Task<HttpResponseMessage> Post(string url, string paramJson)
        {
            var token = GetTokenKey();

            using var content = new StringContent(paramJson, Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsync(url, content);

            return response.StatusCode switch
            {
                HttpStatusCode.OK => response,
                HttpStatusCode.Locked => response,
                HttpStatusCode.Accepted => response,
                HttpStatusCode.Unauthorized => throw new UnauthorizedAccessException("Authentication required"),
                _ => throw new HttpRequestException($"Error {response.StatusCode}: {response.ReasonPhrase}")
            };
        }

        private string? GetTokenKey()
        {
            var token = HttpContext.Request.Cookies["JWT"];
            return !string.IsNullOrEmpty(token) ? token : null;
        }

        private async Task<string?> RefreshJWT()
        {
            var refreshToken = HttpContext.Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            var request = new { RefreshToken = refreshToken };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/v1/auth/refresh", content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var credential = JsonSerializer.Deserialize<CredentialDTO>(responseText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            HttpContext.Response.Cookies.Append("JWT", credential.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration["Session:TimeOut"] ?? "30"))
            });

            return credential.Token;
        }

    }

    public class UserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class ApiToken
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }

    public class LoginInput
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [JsonPropertyName("email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("ipddress")] 
        public string? IpAddress { get; set; }

        [JsonPropertyName("sessionid")]
        public string? SessionId { get; set; }
    }
}