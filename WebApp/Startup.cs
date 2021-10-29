using System;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using ApplicationServices.Helpers;
using ApplicationServices.Services;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Core.Infrastructure.Contexts;
using Core.Infrastructure.Repositories;
using Core.Infrastructure.Seeders.SecurityDb;
using Infrastructure.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IMvcBuilder mvcBuilder = services.AddControllersWithViews();
            if (this.Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                options.SlidingExpiration = true;
            });
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddHttpContextAccessor();

            services.AddHttpClient(
                "default",
                configureClient: (serviceProvider, client) =>
                {
                    client.BaseAddress = new Uri($"{Configuration.GetConnectionString("Web")}/");

                    //If User is authenticated, add token to HTTP client.
                    IHttpContextAccessor httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                    HttpContext httpContext = httpContextAccessor?.HttpContext;

                    if
                    (
                        httpContext != null
                        && httpContext.User.Identity != null
                        && httpContext.User.Identity.IsAuthenticated
                        && httpContext.Session.TryGetValue("token", out byte[] tokenBytes)
                    )
                    {
                        client.DefaultRequestHeaders.Authorization = (
                            new AuthenticationHeaderValue("Bearer", Encoding.ASCII.GetString(tokenBytes))
                        );
                    }
                }
            );
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default")).UseLazyLoadingProxies();
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
            services.AddScoped(typeof(IService<Treatment>), typeof(TreatmentService));
            services.AddScoped(typeof(IService<Comment>), typeof(CommentService));
            services.AddScoped(typeof(IService<Appointment>), typeof(AppointmentService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IAuthHelper), typeof(AuthHelper));
            
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
            
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            
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