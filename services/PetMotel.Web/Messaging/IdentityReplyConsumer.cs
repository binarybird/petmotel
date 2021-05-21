using System;
using System.Threading.Tasks;
using MassTransit;
using PetMotel.Common.Messaging.Exchanges.Identity;

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
