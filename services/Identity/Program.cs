using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Common.Messaging;
using Identity.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .ConfigureLogging(logging =>
                // {
                //     logging.ClearProviders();
                //     logging.AddConsole();
                //     logging.AddConsoleFormatter<>()
                // })
                .ConfigureServices(ConfigureServices);

        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            // services.AddLogging(builder =>
            // {
            //     builder.AddConsole();
            // });
            services.AddHostedService<Worker>();


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
                    
                    cfg.ReceiveEndpoint(RabbitMqConstants.IdentityService, e =>
                    {
                        e.Consumer<LoginConsumer>(() => new LoginConsumer(services));
                    });
                });
            });
            services.AddMassTransitHostedService();
        }
    }
}