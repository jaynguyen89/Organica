using Hidrogen.Models;
using Hidrogen.Services.DatabaseServices;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hidrogen.Services {

    public static class IServiceCollectionExtension {

        public static IServiceCollection RegisterHidrogenServices(this IServiceCollection services) {

            //Register all services here
            services.AddSingleton<HidrogenDbContext>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IHidrogenianService, HidrogenianService>();

            return services;
        }
    }
}
