using Common;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetMotelWeb.Messaging
{
    public class EventConsumer : IConsumer<IExampleEmail>
    {
        public Task Consume(ConsumeContext<IExampleEmail> context)
        {
            throw new NotImplementedException();
        }
    }
}
