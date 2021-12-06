using System;
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
using PetMotel.Identity.Utilities;

namespace PetMotel.Identity.Controllers
{
    
    [Authorize(Roles = Constants.Roles.User)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<PetMotelUser> _userManager;
        private readonly ILogger<TokenController> _logger;
        private readonly IConfiguration _configuration;
        
        public TokenController(ILogger<TokenController> logger,
            UserManager<PetMotelUser> userManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<TokenResponseModel> IsValid([FromQuery] string token)
        {
            return new TokenResponseModel
            {
                Data = "asdf",
                Errors = Array.Empty<string>(),
                Message = "asdf",
                Succeeded = true
            };
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
                return new TokenResponseModel
                {
                    Data = null,
                    Errors = Array.Empty<string>(),
                    Message = "Unable to find user",
                    Succeeded = false
                };
            }
            
            IList<string> roles = await _userManager.GetRolesAsync(user);
            string token = TokenUtil.GetToken(user, roles, _configuration);

            return new TokenResponseModel
            {
                Data = token,
                Errors = Array.Empty<string>(),
                Message = "",
                Succeeded = true
            };
        }
    }
}