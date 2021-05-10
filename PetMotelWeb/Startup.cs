using System;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
            services.AddLogging(opt => { opt.AddConsole(c => { c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] "; }); });

            string rmqUserPath = null;
            string rmqPassPath = null;
            string certPath = null;
            string keyPath = null;
            string caPath = null;
            string rmqUser = null;
            string rmqPass = null;
            string cert = null;
            string key = null;
            try
            {
                rmqUserPath = Environment.GetEnvironmentVariable("RMQU");
                rmqPassPath = Environment.GetEnvironmentVariable("RMQP");

                rmqUser = File.ReadAllText(rmqUserPath);
                rmqPass = File.ReadAllText(rmqPassPath);

                certPath = Environment.GetEnvironmentVariable("CRTP");
                keyPath = Environment.GetEnvironmentVariable("KP");
                caPath = Environment.GetEnvironmentVariable("CAP");

                cert = File.ReadAllText(certPath);
                key = File.ReadAllText(keyPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine(
                $"Got {rmqUserPath}, {rmqUser}, {rmqPassPath}, {rmqPass}, {certPath}, {keyPath}, {caPath}");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Durable = true;
                    cfg.AutoDelete = false;
                    cfg.Exclusive = false;
                    cfg.Host(RabbitMqConstants.GetRabbitMqUri(rmqUser, rmqPass), h =>
                    {
                        h.UseSsl(ssl =>
                        {
                            ssl.ServerName = "cluster.local";
                            ssl.Certificate = X509Certificate2.CreateFromPem(cert, key);
                            ssl.AllowPolicyErrors(SslPolicyErrors.RemoteCertificateNameMismatch |
                                                  SslPolicyErrors.RemoteCertificateChainErrors);
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

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}