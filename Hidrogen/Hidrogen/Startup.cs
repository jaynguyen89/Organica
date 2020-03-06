using HelperLibrary;
using Hidrogen.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Hidrogen {

    public class Startup {

        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services) {
            services.AddCors();
            services.AddControllers();

            services.AddMvc(options => options.EnableEndpointRouting = false)
                    .AddSessionStateTempDataProvider()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "HidrogenSessionData";
            });

            services.RegisterHidrogenServices();
            services.RegisterCommonServices();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();

            //Need attention on AllowAnyHeader and AllowAnyOrigin
            app.UseCors(builder =>
                        builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller}/{action}/{id?}"
                //);
            });
        }
    }
}
