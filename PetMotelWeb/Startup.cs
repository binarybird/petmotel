using System;
using Common;
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
            // var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            // {
            //     config.Host();
            // })
            services.AddLogging(opt =>
            {
                opt.AddConsole(c =>
                {
                    c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
                });
            });
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Durable = true;
                    cfg.AutoDelete = false;
                    cfg.Exclusive = false;
                    cfg.Host(Common.RabbitMqConstants.RabbitMqUri, h=>
                    {
                        h.UseSsl(ssl =>
                        {
                            ssl.Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate("/tls/crt.pem");
                        });
                    });
                    //cfg.ReceiveEndpoint(RabbitMqConstants.IdentityAccountQueue, c =>
                    //{
                    //   c.Handler<IExampleEmail>(ctx =>
                    //    {
                    //        return Console.Out.WriteLineAsync(ctx.Message.Email);
                    //    });
                    //});
                });
            });
            // services.AddMassTransitHostedService();
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();

            // services.AddTransient<MessageManager>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
