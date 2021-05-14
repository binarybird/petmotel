using System;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Common;
using Common.Messaging;
using Common.Messaging.Exchanges.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetMotelWeb.Data;
using MassTransit;
using Microsoft.Extensions.Logging;
using PetMotelWeb.Messaging;

namespace PetMotelWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(opt => { opt.AddConsole(c => { c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] "; }); });

            var (cert, key, rmqUser, rmqPass) = Common.RmqInitializer.Initialize();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(RabbitMqConstants.GetRabbitMqUri(rmqUser, rmqPass), h =>
                    {
                        h.UseSsl(ssl =>
                        {
                            ssl.ServerName = "cluster.local";
                            ssl.Certificate = X509Certificate2.CreateFromPem(cert, key);
                        });
                    });
                });
                
                var timeout = TimeSpan.FromSeconds(10);
                var serviceAddress = new Uri(RabbitMqConstants.GetServiceUri(RabbitMqConstants.IdentityService));

                x.AddRequestClient<ILogin>(serviceAddress, timeout);
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}