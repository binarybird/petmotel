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
                var s = Guid.NewGuid().ToString();
                object l = new
                {
                    UserUuid = s,
                    UserName = user,
                    Password = "blahone",
                    RememberMe = true
                };

                _logger.LogInformation($"Sending: {s}");
                
                var result = await _requestClient.GetResponse<IIdentityReply>(l);

                
                
                Console.Out.WriteLine($"Got reply {result.Message.UserUuid}");
            }
            catch (RequestTimeoutException e)
            {
                Console.Out.WriteLine(e);
            }

            _logger.LogInformation("Done");
        
            RedirectToPage("/");
        }
    }
}
