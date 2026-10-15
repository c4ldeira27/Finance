using Finance.Application.Abstractions.Identity;
using Finance.Infrastructure.Identity;
using Finance.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var cnnString = configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found. Configure it in user secrets or environment variables.");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(cnnString);
            });

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager();

            services.AddScoped<IIdentityService, IdentityService>();

            return services;

        }
    }
}
