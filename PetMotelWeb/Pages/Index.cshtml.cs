using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Messaging.Exchanges.Identity;
using MassTransit;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PetMotelWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBusControl _bus;
        readonly IRequestClient<ILogin> _requestClient;

        public IndexModel(ILogger<IndexModel> logger, IBusControl bus, IRequestClient<ILogin> requestClient)
        {
            _bus = bus;
            _logger = logger;
            _requestClient = requestClient;
        }

        public void OnGet()
        {

        }

        public async void OnPost()
        {
            var user = Request.Form["search"];
            _logger.LogInformation($"Form: {user}");
        
            _logger.LogInformation("Start");
            
            try
            {
                object l = new
                {
                    UserUuid = Guid.NewGuid().ToString(),
                    UserName = user,
                    Password = "blahone",
                    RememberMe = true
                };
                var result = await _requestClient.GetResponse<IIdentityReply>(l);

                Console.Out.WriteLine($"Got {result.Message}");
            }
            catch (RequestTimeoutException e)
            {
                Console.Out.WriteLine(e);
            }
            
            // object l = new
            // {
            //     UserUuid = Guid.NewGuid().ToString(),
            //     UserName = user,
            //     Password = "blahone",
            //     RememberMe = true
            // };
            //
            // object l2 = new
            // {
            //     UserUuid = Guid.NewGuid().ToString(),
            //     UserName = user,
            //     Password = "blahtwo",
            //     RememberMe = true
            // };
            //
            // object l3 = new
            // {
            //     UserUuid = Guid.NewGuid().ToString(),
            //     UserName = user,
            //     Password = "blahthree",
            //     RememberMe = true
            // };
            //
            // var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            // var sendEndpoint = await _bus.GetSendEndpoint(new Uri("queue:identity_login_queue"));
            // await sendEndpoint.Send<ILogin>(l, source.Token);
            //
            // var source2 = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            // var sendEndpoint2 = await _bus.GetPublishSendEndpoint<ILogin>();
            // await sendEndpoint2.Send<ILogin>(l2, source2.Token);
            //
            // var source3 = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            // await _bus.Publish<ILogin>(l3, source3.Token);

            _logger.LogInformation("Done");
        
            RedirectToPage("/");
        }
    }
}
