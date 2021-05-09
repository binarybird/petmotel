using System;
using System.Runtime.Serialization;
using System.Threading;
using MassTransit;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PetMotelWeb.Messaging;

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

        public void OnPost()
        {
            var emailAddress = Request.Form["search"];
            _logger.LogInformation($"Form: {emailAddress}");

            _logger.LogInformation("Start");
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            _bus.Publish<ExampleEmail>(new
            {
                email = emailAddress
            }, source.Token);
            _logger.LogInformation("Done");

            RedirectToPage("/");
        }
    }
}
