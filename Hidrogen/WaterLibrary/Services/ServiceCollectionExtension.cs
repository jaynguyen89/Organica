using Microsoft.Extensions.DependencyInjection;
using WaterLibrary.DbContexts;
using WaterLibrary.Interfaces;

namespace WaterLibrary.Services {

    public static class ServiceCollectionExtension {

        public static IServiceCollection RegisterWaterServices(
            this IServiceCollection services
        ) {
            //Register all services here
            services.AddTransient<WaterDbContext>();

            services.AddScoped<IWaterService, WaterService>();

            return services;
        }
    }
}