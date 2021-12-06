using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetMotel.Common;
using PetMotel.Common.Rest;
using PetMotel.Common.Rest.Entity;
using PetMotel.Common.Rest.Model;
using PetMotel.Common.Rest.Utilities;
using PetMotel.Identity.Data;

namespace PetMotel.Identity.Controllers
{
    [Authorize(Roles = Constants.Roles.Contributor)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<PetMotelUser> _userManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IConfiguration _configuration;
        private readonly PetMotelIdentityContext _context;
        private readonly IUriService _uriService;

        public UsersController(ILogger<UsersController> logger,
            UserManager<PetMotelUser> userManager,
            IConfiguration configuration,
            PetMotelIdentityContext context,
            IUriService uriService)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _uriService = uriService;
        }

        [HttpGet]
        public async Task<PagedResponseModel<List<PetMotelUser>>> Get([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Users
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _context.Users.CountAsync();
            return PaginationUtil.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
        }

        [HttpPost]
        [Authorize(Roles = Constants.Roles.Manager)]
        public async Task<UsersResponseModel> Update(PetMotelUser req)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete]
        [Authorize(Roles = Constants.Roles.Manager)]
        public async Task<UsersResponseModel> Delete(PetMotelUser req)
        {
            throw new NotImplementedException();
        }
    }
}