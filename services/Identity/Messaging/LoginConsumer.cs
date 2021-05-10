using System;
using System.Threading.Tasks;
using Common.Messaging.Exchanges.Identity;
using MassTransit;

namespace Identity.Messaging
{
    public class LoginConsumer : IConsumer<ILogin>
    {
        public LoginConsumer()
        {
            
        }
        
        public async Task Consume(ConsumeContext<ILogin> context)
        {
            ILogin login = context.Message;
            Console.Out.WriteLine($"Got Login: {login.UserName} {login.MessageUuid} {login.Password}");

            Console.Out.WriteLine("Replying");
            await context.Publish<IIdentityReply>(new
            {
                UserUuid = login.UserUuid,
                MessageUuid = Guid.NewGuid().ToString(),
                
                OriginatingMessageUuid = login.MessageUuid,
                StatusCode = 200,
                Success = false,
                Message = "something"
            });
        }
    }
}