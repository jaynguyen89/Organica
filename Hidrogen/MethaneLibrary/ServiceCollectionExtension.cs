using MethaneLibrary.DbContext;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MethaneLibrary {
    
    public static class ServiceCollectionExtension {
        
        public static IServiceCollection RegisterMethaneServices(this IServiceCollection services) {
            
            //Register all services here
            services.AddTransient<MethaneDbContext>();

            services.AddScoped<IRuntimeLogService, RuntimeLogService>();
            services.AddScoped<IAccountLogService, AccountLogService>();

            return services;
        }
    }
}