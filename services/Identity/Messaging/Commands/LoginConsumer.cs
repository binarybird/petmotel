using System;
using System.Threading.Tasks;
using Common.Messaging.Exchanges.Identity;
using MassTransit;

namespace Identity.Messaging.Commands
{
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
            await context.Publish<IIdentityReply>(new
            {
                UserUuid = login.UserUuid,
                StatusCode = 200,
                Success = false,
                Message = "something",
                __ResponseAddress = context.SourceAddress
            });
        }
    }
}