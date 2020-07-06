using System.Linq;
using Elk.AspNetCore;
using $ext_safeprojectname$.EFDataAccess;
using $ext_safeprojectname$.DependencyResolver;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace $ext_safeprojectname$.Dashboard
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddContext<EfDbContext>(_config.GetConnectionString("EfDbContext"));
            services.AddContext<AuthDbContext>(_config.GetConnectionString("AuthDbContext"));

            services.AddMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
            {
                opt.Cookie.SameSite = SameSiteMode.Lax;
            });

            services.AddHttpContextAccessor();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient(_config);
            services.AddScoped(_config);
            services.AddSingleton(_config);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStaticFiles();
            }
            else
            {
                var cachePeriod = env.IsDevelopment() ? "1" : "604800";
                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = ctx => { ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}"); }
                });
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                await next.Invoke();
                if (!context.Request.IsAjaxRequest())
                {
                    var handled = context.Features.Get<IStatusCodeReExecuteFeature>();
                    var statusCode = context.Response.StatusCode;
                    if (handled == null && statusCode >= 400)
                    {
                        context.Response.Redirect($"/Error/Details?code={statusCode}");
                    }
                }

            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Auth}/{action=SignIn}");
            });
        }
    }
}
