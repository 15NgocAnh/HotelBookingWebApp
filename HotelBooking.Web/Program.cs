using HotelBooking.Domain.Authentication;
using HotelBooking.Domain.AutoMapper;
using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Domain.Encryption;
using HotelBooking.Web.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using static HotelBooking.Web.Pages.Abstract.AbstractPageModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container BEFORE building the app
builder.Services.AddRazorPages();

// Add Password Hasher
builder.Services.AddScoped<IPasswordHasher, Bcrypt>();

// Add AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
    cfg.AddProfile(new MappingProfile(passwordHasher));
});

builder.Services.AddDistributedMemoryCache(); // Lưu trữ session trên RAM
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Đăng ký IHttpContextAccessor
builder.Services.AddHttpContextAccessor(); // Cho phép truy cập HttpContext từ Razor Page

// Cấu Hình HttpClient Để Gọi API
builder.Services.AddHttpClient("BackendApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BackendApi"]);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

// Đăng ký dịch vụ CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["AllowedOrigins"] ?? "https://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add Authentication BEFORE building the app
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(
                double.Parse(builder.Configuration["Session:TimeOut"] ?? "30")
            );
        options.SlidingExpiration = true;
    });

// Add Authorization
builder.Services.AddAuthorization();

// Build the app AFTER configuring services
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Check token còn hạn không, nếu hết thì gọi API refresh token
app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["JWT"];
    var refreshToken = context.Request.Cookies["RefreshToken"];

    if (!string.IsNullOrEmpty(token) && JWTHelper.JwtExpired(token) && !string.IsNullOrEmpty(refreshToken))
    {
        using var scope = app.Services.CreateScope();
        var httpClient = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("BackendApi");

        var response = await httpClient.PostAsJsonAsync("api/v1/auth/refresh", new { Token = refreshToken });

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadFromJsonAsync<TokenDTO>();
            var newToken = apiResponse;
            context.Response.Cookies.Append("JWT", newToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });
        }
        else
        {
            // Refresh token không hợp lệ -> Chuyển hướng về trang đăng nhập
            context.Response.Cookies.Delete("JWT");
            context.Response.Cookies.Delete("RefreshToken");
            context.Response.Redirect("/Account/Login");
            return;
        }
    }

    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseSession();

// IMPORTANT: Add these in this order
app.UseAuthentication();
app.UseAuthorization();

// Add our custom authentication middleware
app.UseMiddleware<AuthenticationMiddleware>();

app.MapRazorPages();
app.Run();