using HotelBooking.API.Extensions;
using HotelBooking.API.Middleware;
using HotelBooking.Infrastructure.Config;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

/// <summary>
/// Entry point for the Hotel Booking API application.
/// Configures and sets up the application services and middleware pipeline.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Configure application settings
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Configure database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") 
        ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

// Add services
builder.Services
    .AddApiVersioningConfig()
    .AddSwaggerConfig()
    .AddJwtAuthentication(builder.Configuration)
    .AddCorsConfig()
    .AddControllersConfig()
    .AddAuthorizationConfig()
    .AddApplicationServices();

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<ValidationExceptionHandlerMiddleware>();
app.UseMiddleware<ExpiredTokenMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCorsConfig();
app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCaching();
app.MapControllers();

app.Run();
