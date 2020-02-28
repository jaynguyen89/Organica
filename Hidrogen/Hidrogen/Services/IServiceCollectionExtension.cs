using Hidrogen.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Hidrogen.Services {

    public static class IServiceCollectionExtension {

        public static IServiceCollection RegisterHidrogenServices(this IServiceCollection services) {

            //Register all services here
            services.AddSingleton<HidrogenDbContext>();

            return services;
        }
    }
}
