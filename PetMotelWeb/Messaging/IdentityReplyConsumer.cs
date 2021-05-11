using Common;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging.Exchanges;
using Common.Messaging.Exchanges.Identity;

namespace PetMotelWeb.Messaging
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
