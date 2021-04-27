using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PetMotelWeb.Messaging;

namespace PetMotelWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            var emailAddress = Request.Form["search"];
            _logger.LogInformation($"Got {emailAddress}");
            MessageManager m = new MessageManager(_logger);
            m.SendExampleEmail(new ExampleEmail(emailAddress));

            RedirectToPage("/");
        }
    }
}
