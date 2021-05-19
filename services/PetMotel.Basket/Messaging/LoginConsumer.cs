using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging.Exchanges.Identity;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PetMotel.Basket.Messaging
{
    public class LoginConsumer : IConsumer<ILogin>
    {
        private readonly ILogger<LoginConsumer> _logger;

        public LoginConsumer()
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                ILoggerFactory loggerFactory = serviceScope.ServiceProvider.GetService<ILoggerFactory>();
                _logger = loggerFactory.CreateLogger<LoginConsumer>();
            }
        }

        public async Task Consume(ConsumeContext<ILogin> context)
        {
            ILogin login = context.Message;

            _logger.LogInformation($"Got LoginConsumer: {login.UserName} {login.Password} {login.UserUuid}");
            
            _logger.LogInformation("Replying");

            await context.RespondAsync<IIdentityReply>(new
            {
                UserUuid = login.UserUuid,
                StatusCode = 200,
                Success = false,
                Message = "something",
            });
            
            _logger.LogInformation("Done");
        }
    }
}