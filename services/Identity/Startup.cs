using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Common;
using Common.Messaging;
using Common.Messaging.Exchanges;
using Identity.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Identity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer();
            
            var (cert, key, rmqUser, rmqPass) = Common.RmqInitializer.Initialize();

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
                        });
                    });
                    cfg.ReceiveEndpoint("identity_login_queue", e =>
                    {
                        e.BindQueue = true;
                        e.Consumer<LoginConsumer>();
                    });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });
        }
    }
}