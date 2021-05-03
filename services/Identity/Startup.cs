using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer();
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Durable = true;
                    cfg.AutoDelete = false;
                    cfg.Exclusive = false;
                    cfg.Host(Common.RabbitMqConstants.RabbitMqUri);
                    cfg.ReceiveEndpoint(RabbitMqConstants.IdentityLoginLogoutQueue, c =>
                    {
                        c.Bind(RabbitMqConstants.IdentityExchange);
                        c.Handler<IExampleEmail>(ctx => { return Console.Out.WriteLineAsync(ctx.Message.Email); });
                    });
                });
            });
            services.AddMassTransitHostedService();
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