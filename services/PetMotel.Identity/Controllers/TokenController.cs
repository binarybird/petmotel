using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using PetMotel.Common;
using PetMotel.Common.Rest.Entity;
using PetMotel.Common.Rest.Model;

namespace PetMotel.Identity.Controllers
{
    
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<PetMotelUser> _userManager;
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;
        
        public TokenController(ILogger<LoginController> logger,
            UserManager<PetMotelUser> userManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = Constants.Roles.Root)]
        public async Task<TokenResponseModel> AdminOnly()
        {
            return new TokenResponseModel("admin only", true, true);
        }

        [HttpPost]
        public async Task<TokenResponseModel> RefreshToken()
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()
            );

            if (user == null)
            {
                return new TokenResponseModel(null, false, false);
            }
            
            IList<string> roles = await _userManager.GetRolesAsync(user);
            string token = TokenUtil.GetToken(user, roles, _configuration);

            return new TokenResponseModel(token, true, true);
        }
    }
}