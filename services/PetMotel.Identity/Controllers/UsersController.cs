using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetMotel.Common;
using PetMotel.Common.Rest.Entity;
using PetMotel.Common.Rest.Model;

namespace PetMotel.Identity.Controllers
{
    [Authorize(Roles = Constants.Roles.Contributor)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController
    {
        private readonly UserManager<PetMotelUser> _userManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IConfiguration _configuration;
        
        public UsersController(ILogger<UsersController> logger,
            UserManager<PetMotelUser> userManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }
        
        [HttpGet]
        public async Task<UsersResponseModel> Get(UsersRequestModel req)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [Authorize(Roles = Constants.Roles.Manager)]
        public async Task<UsersResponseModel> Post(UsersRequestModel req)
        {
            throw new NotImplementedException();
        }
    }
}