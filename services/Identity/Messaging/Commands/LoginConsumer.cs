using System;
using System.Threading.Tasks;
using Common.Messaging.Exchanges.Identity;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace Identity.Messaging.Commands
{
    public class LoginConsumerDefinition : ConsumerDefinition<LoginConsumer>
    {
        public LoginConsumerDefinition()
        {
            EndpointName = "identity_reply_queue";

            ConcurrentMessageLimit = 8;
        }
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<LoginConsumer> consumerConfigurator)
        {
            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);
        }
    }
    public class LoginConsumer : IConsumer<ILogin>
    {
        public LoginConsumer()
        {
            
        }
        
        public async Task Consume(ConsumeContext<ILogin> context)
        {
            ILogin login = context.Message;
            Console.Out.WriteLine($"Got Login: {login.UserName} {login.Password} {login.UserUuid}");
            
            Console.Out.WriteLine("Replying");

            await context.RespondAsync<IIdentityReply>(new
            {
                UserUuid = login.UserUuid,
                StatusCode = 200,
                Success = false,
                Message = "something",
            });
            
            Console.Out.WriteLine("Done");

            //await context.GetSendEndpoint(new Uri("queue:identity_reply_queue"));
            
            // await context.Publish<IIdentityReply>(new
            // {
            //     UserUuid = login.UserUuid,
            //     StatusCode = 200,
            //     Success = false,
            //     Message = "something",
            //     // __ResponseAddress = context.SourceAddress
            // });
        }
    }
}