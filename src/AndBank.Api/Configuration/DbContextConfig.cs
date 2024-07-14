using AndBank.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AndBank.Api.Configuration
{
    public static class DbContextConfig
    {
        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<PositionDbContext>();

            return builder;
        }
    }
}
