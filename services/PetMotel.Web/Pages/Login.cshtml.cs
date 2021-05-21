using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PetMotel.Common.Messaging.Exchanges.Identity;
using PetMotel.Common.Rest.Model;

namespace PetMotel.Web.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public PetMotelLoginModel PMLoginModel { get; set; }

        private ILogger<LoginModel> _logger;
        private IBusControl _bus;
        private IRequestClient<ILogin> _requestClient;
        
        public LoginModel(ILogger<LoginModel> logger, IBusControl bus, IRequestClient<ILogin> requestClient)
        {
            _logger = logger;
            _bus = bus;
            _requestClient = requestClient;
        }
        
        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            if (this.ModelState.IsValid)
            {
                ClaimsPrincipal principal = this.User;

                int y = 0;
            }
            else
            {
                int i = 0;
            }

        }
    }
}