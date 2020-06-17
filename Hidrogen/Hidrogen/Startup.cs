using System;
using HelperLibrary;
using MethaneLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WaterLibrary.Services;
using ServiceCollectionExtension = Hidrogen.Services.ServiceCollectionExtension;

namespace Hidrogen {

    public class Startup {
        
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {

            services.AddAntiforgery(options => {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            services.AddCors();
            services.AddControllers();

            services.AddMvc(options => options.EnableEndpointRouting = false)
                    .AddSessionStateTempDataProvider()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddHttpContextAccessor();

            ServiceCollectionExtension.RegisterHidrogenServices(services);

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = Configuration.GetSection("RedisCaching")["Connection"];
                options.InstanceName = Configuration.GetSection("RedisCaching")["HidroData"];
            });
            
            services.RegisterCommonServices();
            services.RegisterWaterServices();
            
            services.Configure<ServerOptions>(Configuration.GetSection("MongoServer"));
            services.RegisterMethaneServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            var securityPolicies = new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders()
                .AddContentSecurityPolicy(builder => {
                    builder.AddUpgradeInsecureRequests();
                    builder.AddDefaultSrc().Self().OverHttps().From("https://localhost:5001/");
                });

            app.UseSecurityHeaders(securityPolicies);
            app.UseHttpsRedirection();
            app.UseRouting();

            //Need attention on AllowAnyHeader and AllowAnyOrigin
            app.UseCors(builder =>
                        builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithOrigins("http://localhost:3000"));

            app.UseAuthorization();
            app.UseSession();

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}"
                );
            });
        }
    }
}
