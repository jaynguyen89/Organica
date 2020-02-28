using Hidrogen.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "HidrogenSessionData";
            });

            services.RegisterHidrogenServices();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            //Need attention on AllowAnyHeader and AllowAnyOrigin
            app.UseCors(builder =>
                        builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
