using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FunWithin.Models;
using FunWithin.Controllers;

namespace FunWithin
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) =>
            Configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContextPostgres>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("FunWithin2")));
            //services.AddSingleton<MetaWebHostinhEnvironment>();
            services.AddTransient<IReviewRepository, EFReviewRepository>();
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: null,
                template: "{type}/Page{reviewPage:int}",
                defaults: new { controller = "Review", action = "List" }
                );
                routes.MapRoute(
                    name: null,
                    template: "Page{reviewPage:int}",
                    defaults: new { controller = "Review", action = "List", reviewPage = 1 });
                routes.MapRoute(
                    name: null,
                    template: "{type}",
                    defaults: new { controller = "Review", action = "List", reviewPage = 1 });
                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { controller = "Review", action = "List", reviewPage = 1 });
                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });
        }


    }
}
