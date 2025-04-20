using Asp.Versioning;
using AutoMapper;
using Hangfire;
using HotelBooking.API.Middleware;
using HotelBooking.API.Policy;
using HotelBooking.API.Policy.Requirement;
using HotelBooking.Data;
using HotelBooking.Data.Config;
using HotelBooking.Domain.Authentication;
using HotelBooking.Domain.AutoMapper;
using HotelBooking.Domain.DTOs.Post;
using HotelBooking.Domain.Email;
using HotelBooking.Domain.Encryption;
using HotelBooking.Domain.Filtering;
using HotelBooking.Domain.Repository;
using HotelBooking.Domain.Repository.Interfaces;
using HotelBooking.Domain.Services;
using HotelBooking.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

/// <summary>
/// Entry point for the Hotel Booking API application.
/// Configures and sets up the application services and middleware pipeline.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Configure application settings
ConfigureApplicationSettings(builder);

// Configure API versioning
ConfigureApiVersioning(builder);

// Configure database services
ConfigureDatabaseServices(builder);

// Configure JWT authentication
ConfigureJwtAuthentication(builder);

// Configure application services
ConfigureApplicationServices(builder);

// Configure Hangfire for background jobs
ConfigureHangfire(builder);

// Configure filtering services
ConfigureFilteringServices(builder);

// Configure repositories
ConfigureRepositories(builder);

// Configure Swagger
ConfigureSwagger(builder);

// Configure authorization policies
ConfigureAuthorizationPolicies(builder);

// Configure AutoMapper
ConfigureAutoMapper(builder);

// Configure CORS
ConfigureCors(builder);

var app = builder.Build();

// Configure the HTTP request pipeline
ConfigureHttpPipeline(app);

app.Run();

#region Configuration Methods

/// <summary>
/// Configures application settings from appsettings.json
/// </summary>
static void ConfigureApplicationSettings(WebApplicationBuilder builder)
{
    builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
    builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("AppSettings"));
    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
}

/// <summary>
/// Configures API versioning for the application
/// </summary>
static void ConfigureApiVersioning(WebApplicationBuilder builder)
{
    var apiVersioningBuilder = builder.Services.AddApiVersioning(o =>
    {
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.ReportApiVersions = true;
    });

    apiVersioningBuilder.AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
}

/// <summary>
/// Configures database services and connection
/// </summary>
static void ConfigureDatabaseServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") 
            ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));
}

/// <summary>
/// Configures JWT authentication settings
/// </summary>
static void ConfigureJwtAuthentication(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings")["Secret"] 
                        ?? throw new InvalidOperationException("'Secret' not found.")))
            };
        });
}

/// <summary>
/// Configures application services and dependencies
/// </summary>
static void ConfigureApplicationServices(WebApplicationBuilder builder)
{
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSingleton<IPasswordHasher, Bcrypt>();
    builder.Services.AddSingleton<IJWTHelper, JWTHelper>();
    builder.Services.AddTransient<IEmailSender, EmailSenderServices>();
    builder.Services.AddScoped<IUserServices, UserServices>();
    builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
    builder.Services.AddScoped<IJwtServices, JwtServices>();
    builder.Services.AddScoped<IAuthorizationHandler, EmailVerifiedHandler>();
    builder.Services.AddTransient<IEmailServices, EmailServices>();
    builder.Services.AddTransient<IPostService, PostService>();
    builder.Services.AddTransient<IRoomService, RoomService>();
    builder.Services.AddTransient<IRoomTypeService, RoomTypeService>();
    builder.Services.AddTransient<IBookingService, BookingService>();
    
    builder.Services.AddControllers()
        .AddJsonOptions(opt => 
        { 
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
        });
    
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
}

/// <summary>
/// Configures Hangfire for background job processing
/// </summary>
static void ConfigureHangfire(WebApplicationBuilder builder)
{
    builder.Services.AddHangfire(configuration => configuration
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("AppDbContext")));
    builder.Services.AddHangfireServer();
}

/// <summary>
/// Configures filtering services for data queries
/// </summary>
static void ConfigureFilteringServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IFilterHelper<PostValidatorDTO>, FilterHelper<PostValidatorDTO>>();
    builder.Services.AddScoped<IFilterHelper<PostDetailsDTO>, FilterHelper<PostDetailsDTO>>();
}

/// <summary>
/// Configures repository services
/// </summary>
static void ConfigureRepositories(WebApplicationBuilder builder)
{
    builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    builder.Services.AddTransient<IRoleRepository, RoleRepository>();
    builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
    builder.Services.AddTransient<IJwtRepository, JwtRepository>();
    builder.Services.AddTransient<IPostRepository, PostRepository>();
    builder.Services.AddTransient<IRoomRepository, RoomRepository>();
    builder.Services.AddTransient<IRoomTypeRepository, RoomTypeRepository>();
    builder.Services.AddTransient<IBookingRepository, BookingRepository>();
}

/// <summary>
/// Configures Swagger documentation
/// </summary>
static void ConfigureSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                          Enter 'Bearer' [space] and then your token in the text input below.
                          \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    });
}

/// <summary>
/// Configures authorization policies
/// </summary>
static void ConfigureAuthorizationPolicies(WebApplicationBuilder builder)
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("emailverified", 
            policy => policy.Requirements.Add(new EmailVerifiedRequirement()));
    });
}

/// <summary>
/// Configures AutoMapper for object mapping
/// </summary>
static void ConfigureAutoMapper(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton(provider => new MapperConfiguration(options =>
    {
        options.AddProfile(new MappingProfile(provider.GetService<IPasswordHasher>()));
    }).CreateMapper());
}

/// <summary>
/// Configures CORS policies
/// </summary>
static void ConfigureCors(WebApplicationBuilder builder)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowWebApp", policy =>
        {
            policy.WithOrigins("https://localhost:5001")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
}

/// <summary>
/// Configures the HTTP request pipeline
/// </summary>
static void ConfigureHttpPipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionHandlerMiddleware>();
    app.UseMiddleware<ValidationExceptionHandlerMiddleware>();
    app.UseMiddleware<ExpiredTokenMiddleware>();

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseCors("AllowWebApp");
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

#endregion
