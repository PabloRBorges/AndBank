using Microsoft.OpenApi.Models;

namespace ApiFuncional.Configuration
{
    public static class SwaggerConfig
    {
        public static WebApplicationBuilder AddSwaggerConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AndBank API", Version = "v1" })
            );

            return builder;
        }
    }
}
