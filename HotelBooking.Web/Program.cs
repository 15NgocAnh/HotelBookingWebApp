using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container BEFORE building the app
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Đăng ký IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseSession();

// IMPORTANT: Add these in this order
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();