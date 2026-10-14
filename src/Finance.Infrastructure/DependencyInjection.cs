using Finance.Infrastructure.Persistence;
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
        public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            var cnnString = configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found. Configure it in user secrets or environment variables.");

            service.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(cnnString);
            });

            return service;

        }
    }
}
