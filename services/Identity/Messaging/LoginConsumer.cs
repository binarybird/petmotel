using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging.Exchanges.Identity;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Identity.Messaging
{
    public class LoginConsumer : IConsumer<ILogin>
    {
        private readonly ILogger<LoginConsumer> _logger;

        public LoginConsumer(IServiceCollection services)
        {
            object? first = services.Where(w => w.ServiceType == typeof(Microsoft.Extensions.Logging.ILoggerFactory))
                .Select(s => s.ImplementationInstance).First();
            if (first is ILoggerFactory factory)
            {
                _logger = factory.CreateLogger<LoginConsumer>();
            }
            else
            {
                _logger = null;
            }
        }

        public async Task Consume(ConsumeContext<ILogin> context)
        {
            ILogin login = context.Message;

            Console.Out.Write("Login consumer called");
            _logger?.LogInformation($"Got LoginConsumer: {login.UserName} {login.Password} {login.UserUuid}");

            _logger?.LogInformation("Replying");


            await context.RespondAsync<IIdentityReply>(new
            {
                UserUuid = login.UserUuid,
                StatusCode = 200,
                Success = false,
                Message = "something",
            });

            _logger?.LogInformation("Done");
        }
    }
}