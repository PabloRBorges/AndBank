using AndBank.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

namespace AndBank.Api.Configuration
{
    public static class DbContextConfig
    {
        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            string environment;
#if DEBUG
            environment = "Development";
#else
        environment = "Production";
#endif

            Console.WriteLine($"Variável de ambiente: {environment}");

            builder.Configuration
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Console.WriteLine($"Connectionstring: {builder.Configuration.GetConnectionString("DefaultConnection")}");

            builder.Services.AddDbContext<PositionDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            return builder;
        }
    }
}
