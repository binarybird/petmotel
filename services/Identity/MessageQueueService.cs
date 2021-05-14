using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Common.Messaging;
using Common.Messaging.Exchanges.Identity;
using Identity.Messaging.Commands;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Identity
{
    public class MessageQueueService : BackgroundService
    {
        readonly IBusControl _bus;
        
        
        public MessageQueueService()
        {
            var (cert, key, rmqUser, rmqPass) = Common.RmqInitializer.Initialize();
            _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
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
                    // e.Handler<ILogin>(context =>
                    // {
                    //     Console.WriteLine("Got: {0}", context.Message.UserUuid);
                    //
                    //     return context.RespondAsync<IIdentityReply>(new
                    //     {
                    //         UserUuid = context.Message.UserUuid,
                    //         StatusCode = 200,
                    //         Success = false,
                    //         Message = "something",
                    //     });
                    // });
                });
            });
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _bus.StartAsync(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(base.StopAsync(cancellationToken), _bus.StopAsync(cancellationToken));
        }
        
    }
    
    
}