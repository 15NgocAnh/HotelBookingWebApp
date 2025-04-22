using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Infrastructure.Services;
using HotelBooking.Infrastructure.Authorization;

namespace HotelBooking.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // ... existing service registrations ...

            // Register role service
            services.AddScoped<IRoleService, RoleService>();

            // Configure authorization
            services.AddAuthorization(options =>
            {
                // Add policies for different permissions
                options.AddPolicy("CanManageKaraoke", policy =>
                    policy.Requirements.Add(new PermissionRequirement("MANAGE_KARAOKE")));
                
                options.AddPolicy("CanManageHotel", policy =>
                    policy.Requirements.Add(new PermissionRequirement("MANAGE_HOTEL")));
                
                options.AddPolicy("CanManageRestaurant", policy =>
                    policy.Requirements.Add(new PermissionRequirement("MANAGE_RESTAURANT")));
            });

            // Register authorization handler
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ... existing middleware configuration ...

            app.UseAuthentication();
            app.UseAuthorization();

            // ... rest of the configuration ...
        }
    }
} 