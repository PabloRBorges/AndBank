
using AndBank.Business.Interfaces;
using AndBank.Business.Services;
using AndBank.Data.Repository;
using AndBank.Process.Application.AutoMapper;

namespace AndBank.Api.Configuration
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //repository
            services.AddScoped<IPositionRepository,PositionRepository>();

            //services
            services.AddScoped<IPositionService, PositionService>();

            
        }
    }
}
