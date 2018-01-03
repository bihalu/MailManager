using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MailManager.Data;
using MailManager.Security;

namespace MailManager
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(
                options => options.UseMySql(Configuration.GetConnectionString("DatabaseConnection"))
            );

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.ExpireTimeSpan = TimeSpan.FromDays(7));

            services.AddAuthorization(options =>
                options.AddPolicy("IsAdmin", policy =>
                    policy.RequireRole("Admin")));

            services.AddScoped<IUserManager, UserManager>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseStatusCodePages();

            app.UseMvcWithDefaultRoute();
        }
    }
}
