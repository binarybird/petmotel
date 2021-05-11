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

        public IndexModel(ILogger<IndexModel> logger, IBusControl bus)
        {
            _bus = bus;
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async void OnPost()
        {
            var user = Request.Form["search"];
            _logger.LogInformation($"Form: {user}");
        
            _logger.LogInformation("Start");
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri("queue:identity_login_queue"));
            await sendEndpoint.Send<ILogin>(new
            {
                UserUuid = Guid.NewGuid().ToString(),
                UserName = user,
                Password = "blah",
                RememberMe = true
            }, source.Token);

            _logger.LogInformation("Done");
        
            RedirectToPage("/");
        }
    }
}
