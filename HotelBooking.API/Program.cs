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

var builder = WebApplication.CreateBuilder(args);

#region Add Config 
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
#endregion

#region add Version
var apiVersioningBuilder = builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
});

apiVersioningBuilder.AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
#endregion

#region Add DB service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));
#endregion

#region Add JWT Settings 
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

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings")["Secret"] ?? throw new InvalidOperationException("'Secret' not found.")))
        };
    });
#endregion
// Add services to the container.
#region Add Services 
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
builder.Services.AddControllers()
    .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

#endregion

#region add Hangfire 
builder.Services.AddHangfire(configuration => configuration
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("AppDbContext")));
builder.Services.AddHangfireServer();
#endregion

#region  Paging & Sorting on Web-Request
builder.Services.AddScoped<IFilterHelper<PostValidatorDTO>, FilterHelper<PostValidatorDTO>>();
builder.Services.AddScoped<IFilterHelper<PostDetailsDTO>, FilterHelper<PostDetailsDTO>>();
#endregion

#region Repositories
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddTransient<IJwtRepository, JwtRepository>();
builder.Services.AddTransient<IPostRepository, PostRepository>();
#endregion

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

#region config Swagger 
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
#endregion

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("emailverified", policy => policy.Requirements.Add(new EmailVerifiedRequirement()));
});

#region Auto mapper
builder.Services.AddSingleton(provider => new MapperConfiguration(options =>
{
    options.AddProfile(new MappingProfile(provider.GetService<IPasswordHasher>()));
})
.CreateMapper());

#endregion

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins("https://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region some sort of Middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<ValidationExceptionHandlerMiddleware>();
app.UseMiddleware<ExpiredTokenMiddleware>();
#endregion

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowWebApp");
app.UseAuthorization();

app.MapControllers();

app.Run();
