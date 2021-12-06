using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FunWithin.Models;
using Microsoft.AspNetCore.Identity;

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
            options.UseNpgsql(Configuration.GetConnectionString("FunWithin")));
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("FunWithinIdentity")));
            services.AddIdentity<User, IdentityRole>(options => {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = true;

            })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            

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
            app.UseAuthentication();
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
