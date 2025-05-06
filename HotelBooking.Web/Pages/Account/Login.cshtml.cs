using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Web.Pages.Abstract;
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
    public class LoginModel : AbstractPageModel
    {
        public LoginModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public UserLoginDTO Input { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public string? RedirectUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                RedirectUrl = ReturnUrl;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
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
                    new Claim(ClaimTypes.Role, userInfo.RoleName),
                    new Claim(ClaimTypes.Email, userInfo.Email),
                    new Claim("FirstName", userInfo.FirstName),
                    new Claim("LastName", userInfo.LastName),
                    new Claim("LocationAccess", "true") // Add location access claim
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration["Session:TimeOut"] ?? "30")),
                    IsPersistent = false,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                IsSuccess = true;
                RedirectUrl = !string.IsNullOrEmpty(ReturnUrl) ? ReturnUrl : "/Index";
                return Page();
            }
            catch
            {
                ErrorMessage = "An error occurred during login. Please try again.";
                return Page();
            }
        }

        /// <summary>
        /// Gửi request đến API để lấy JWT token khi đăng nhập
        /// </summary>
        private async Task<ApiResponse<CredentialDTO>?> GetJWT()
        {
            try
            {
                // Chuyển dữ liệu Input (email, password) thành JSON để gửi lên API
                var content = new StringContent(JsonSerializer.Serialize(Input), Encoding.UTF8, "application/json"); // Gửi request đến API

                var response = await _httpClient.PostAsync("api/v1/auth/login", content); // Lấy dữ liệu phản hồi từ API
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) // Nếu API trả về lỗi
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (errorResponse?.Errors != null)
                    {
                        // Gộp các lỗi lại thành một chuỗi và gán vào `ErrorMessage`
                        ErrorMessage = string.Join("<br>", errorResponse.Errors.SelectMany(e => e.Value));
                    }
                    return null;
                }

                var credential = JsonSerializer.Deserialize<ApiResponse<CredentialDTO>>(responseText, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                // Nếu API trả về lỗi hoặc không có dữ liệu token -> Không lưu vào cookie
                if (credential == null || string.IsNullOrEmpty(credential.Data.Token))
                {
                    ErrorMessage = "Incorrect account or password!";
                    return null;
                }

                // Lưu JWT vào Cookie để xác thực người dùng
                HttpContext.Response.Cookies.Append("JWT", credential.Data.Token, new CookieOptions
                {
                    HttpOnly = true,  // Chống XSS
                    Secure = true,    // Chỉ hoạt động trên HTTPS
                    SameSite = SameSiteMode.Strict, // Chống CSRF
                    Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration["Session:TimeOut"] ?? "30"))
                });

                // Lưu Refresh Token để tự động làm mới phiên đăng nhập
                HttpContext.Response.Cookies.Append("RefreshToken", credential.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7) // Thời gian dài hơn JWT
                });

                return credential;
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "System error, please try again.");
                return null;
            }
        }

        /// <summary>
        /// Giải mã JWT token để lấy thông tin user
        /// </summary>
        private UserInfo GetUserInfoFromJWT(string jwt)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwt) as JwtSecurityToken;

                return new UserInfo
                {
                    Id = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "",
                    RoleName = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "",
                    Email = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "",
                    FirstName = jsonToken?.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value ?? "",
                    LastName = jsonToken?.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value ?? ""
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
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string RoleName { get; set; } = CJConstant.EMPLOYEE;
    }
}