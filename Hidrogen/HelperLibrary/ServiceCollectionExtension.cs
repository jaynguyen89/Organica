using HelperLibrary.Interfaces;
using HelperLibrary.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HelperLibrary {

    public static class ServiceCollectionExtension {

        public static IServiceCollection RegisterCommonServices(this IServiceCollection services) {

            //Add all services here
            services.AddScoped<IGoogleReCaptchaService, GoogleReCaptchaService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            return services;
        }
    }
}
