using Hidrogen.DbContexts;
using Hidrogen.Services.DatabaseServices;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hidrogen.Services {

    public static class ServiceCollectionExtension {

        public static IServiceCollection RegisterHidrogenServices(this IServiceCollection services) {
            //Register all services here
            services.AddScoped<HidrogenDbContext>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IHidrogenianService, HidrogenianService>();
            services.AddScoped<IHidroProfileService, HidroProfileService>();
            services.AddScoped<IHidroRoleService, HidroRoleService>();
            services.AddScoped<IRoleClaimerService, RoleClaimerService>();
            services.AddScoped<IHidroAddressService, HidroAddressService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ITraderService, TraderService>();

            return services;
        }
    }
}
