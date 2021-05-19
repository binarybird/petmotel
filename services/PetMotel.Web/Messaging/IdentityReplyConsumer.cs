using System;
using System.Threading.Tasks;
using Common.Messaging.Exchanges.Identity;
using MassTransit;

namespace PetMotel.Web.Messaging
{
    public class IdentityReplyConsumer : IConsumer<IIdentityReply>
    {
        public IdentityReplyConsumer()
        {
            
        }
        public async Task Consume(ConsumeContext<IIdentityReply> context)
        {
            IIdentityReply reply = context.Message;
            await Console.Out.WriteLineAsync($"Got Reply: {reply.Message} {reply.UserUuid}");
        }
    }
}
