using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetMotel.Common;
using PetMotel.Common.Rest.Entity;
using PetMotel.Common.Rest.Model;
using PetMotel.Identity.Data;

namespace PetMotel.Identity.Controllers
{
    //or
    //[Authorize(Roles = "HRManager,Finance")]
    
    //and
    //[Authorize(Roles = "PowerUser")]
    //[Authorize(Roles = "ControlPanelUser")]
    //[Authorize(Roles = Roles.Root)]
    [Authorize(Roles = Constants.Roles.User)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly PetMotelIdentityContext _context;
        private readonly UserManager<PetMotelUser> _userManager;
        private readonly SignInManager<PetMotelUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<PetMotelUser> signInManager,
            ILogger<AccountController> logger,
            UserManager<PetMotelUser> userManager,
            PetMotelIdentityContext context,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<AccountResponseModel> Update(PetMotelUser update)
        {
            PetMotelUser user = await _userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()
            );

            if (user == null)
            {
                return new AccountResponseModel(null, false);
            }
            
            //TODO update

            throw new NotImplementedException();
        }
        
        [HttpGet]
        public async Task<AccountResponseModel> Get()
        {
            PetMotelUser user = await _userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()
            );

            if (user == null)
            {
                return new AccountResponseModel(null, false);
            }

            return new AccountResponseModel(user, true);
        }
        
        [HttpDelete]
        public async Task<AccountResponseModel> Delete()
        {
            throw new NotImplementedException();
        }
    }
}