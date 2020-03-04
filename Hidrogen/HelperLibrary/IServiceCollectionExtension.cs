using HelperLibrary.Interfaces;
using HelperLibrary.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HelperLibrary {

    public static class IServiceCollectionExtension {

        public static IServiceCollection RegisterCommonServices(this IServiceCollection services) {

            //Add all services here
            services.AddScoped<IGoogleReCaptchaService, GoogleReCaptchaService>();

            return services;
        }
    }
}
