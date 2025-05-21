using HotelBooking.API.Extensions;
using HotelBooking.API.Middleware;
using HotelBooking.Infrastructure.Config;
using HotelBooking.Infrastructure.Data;

/// <summary>
/// Entry point for the Hotel Booking API application.
/// Configures and sets up the application services and middleware pipeline.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Configure application settings
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Configure database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") 
        ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

// Add services
builder.Services
    .AddSwaggerConfig()
    .AddJwtAuthentication(builder.Configuration)
    .AddCorsConfig()
    .AddControllersConfig()
    .AddAuthorizationConfig()
    .AddApplicationServices();

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (FluentValidation.ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
            );

        var result = new
        {
            status = 400,
            errors
        };

        await context.Response.WriteAsJsonAsync(result);
    }
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCorsConfig();
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<ValidationExceptionHandlerMiddleware>();
app.UseMiddleware<ExpiredTokenMiddleware>();
app.UseMiddleware<RefreshTokenMiddleware>();

app.Run();
