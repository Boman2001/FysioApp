using System;
using System.Security;
using System.Text;
using ApplicationServices.Services;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Core.Infrastructure.Contexts;
using Core.Infrastructure.Repositories;
using Infrastructure.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SendGrid;

namespace WebApp
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration,IWebHostEnvironment environment) {
            
            this.Configuration = configuration;
            this.Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           IMvcBuilder mvcBuilder = services.AddControllersWithViews();
            if (this.Environment.IsDevelopment()) {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddDbContext<SecurityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Security")));

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    config.User.RequireUniqueEmail = true;
                    config.Password.RequireDigit = false;
                    config.Password.RequiredLength = 4;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequiredUniqueChars = 0;
                    config.Password.RequireLowercase = false;
                    config.User.RequireUniqueEmail = true;
                }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddOptions();
            services.Configure<SecurityStampValidatorOptions>(options =>
                options.ValidationInterval = TimeSpan.FromMinutes(5));
            services.AddScoped<IIdentityRepository, IdentityRepository>();

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(DatabaseRepository<>));
            services.AddScoped(typeof(IWebRepository<TreatmentCode>), typeof(WebRepository<TreatmentCode>));
            services.AddScoped(typeof(IWebRepository<DiagnoseCode>), typeof(WebRepository<DiagnoseCode>));
            services.AddScoped(typeof(IService<Patient>), typeof(PatientService));
            services.AddScoped(typeof(IService<Dossier>), typeof(DossierService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
            app.Use(async (context, next) =>
            {
                if (!context.User.Identity.IsAuthenticated
                    && context.Request.Path.StartsWithSegments("/Uploads"))
                {
                    context.Response.Redirect("Account/Login");
                    throw new SecurityException("Not Authorized");
                }
                await next.Invoke();
            });
        }
    }
}