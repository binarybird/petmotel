using System;
using System.IO;
using System.Linq;
using System.Net.Security;
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
            services.AddLogging(opt =>
            {
                opt.AddConsole(c =>
                {
                    c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
                });
            });

            string user;
            string pass;
            string certPath = null;
            string keyPath = null;
            string caPath = null;
            try
            {
                user = Environment.GetEnvironmentVariable("RMQU");
                pass = Environment.GetEnvironmentVariable("RMQP");
                certPath = Environment.GetEnvironmentVariable("CRTP");
                keyPath = Environment.GetEnvironmentVariable("KP");
                caPath = Environment.GetEnvironmentVariable("CAP");
                if (String.IsNullOrEmpty(certPath))
                {
                    certPath = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                user = "null";
                pass = "null";
            }
            
            Console.WriteLine($"Got {user}, {pass}, {certPath}, {keyPath}, {caPath}");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Durable = true;
                    cfg.AutoDelete = false;
                    cfg.Exclusive = false;
                    cfg.Host(RabbitMqConstants.GetRabbitMqUri(user, pass), h=>
                    {
                        if (certPath != null)
                        {
                            h.UseSsl(ssl =>
                            {
                                ssl.ServerName = "cluster.local";
                                ssl.Certificate =
                                    new System.Security.Cryptography.X509Certificates.X509Certificate(certPath);
                                // ssl.AllowPolicyErrors(SslPolicyErrors.RemoteCertificateNameMismatch |
                                //                       SslPolicyErrors.RemoteCertificateChainErrors);
                            });
                        }
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
