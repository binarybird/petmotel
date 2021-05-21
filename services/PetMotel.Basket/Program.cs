using System.Security.Cryptography.X509Certificates;
using PetMotel.Common.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetMotel.Basket.Messaging;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace PetMotel.Basket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost build = CreateHostBuilder(args).Build();
            
            ServiceActivator.Configure(build.Services);
            
            build.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);
        
        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            var (cert, key, rmqUser, rmqPass) = RmqInitializer.Initialize();
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
                        e.Consumer<LoginConsumer>();
                    });
                });
            });
            services.AddMassTransitHostedService();
        }
    }
}