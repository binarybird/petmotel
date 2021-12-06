using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PetMotel.Common;
using PetMotel.Common.Rest.Entity;
using PetMotel.Common.Rest.Model;
using PetMotel.Identity.Data;
using PetMotel.Identity.Utilities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace PetMotel.Identity.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly PetMotelIdentityContext _context;
        private readonly UserManager<PetMotelUser> _userManager;
        private readonly SignInManager<PetMotelUser> _signInManager;
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;

        public LoginController(SignInManager<PetMotelUser> signInManager,
            RoleManager<PetMotelRole> roleManager,
            ILogger<LoginController> logger,
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

        // POST: api/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoginResponseModel>> PostLoginModel(LoginRequestModel petMotelLoginModel)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(
                petMotelLoginModel.Email,
                petMotelLoginModel.Password,
                petMotelLoginModel.RememberMe,
                lockoutOnFailure: false);

            LoginResponseModel resp;

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                var user = await _userManager.FindByNameAsync(petMotelLoginModel.Email);
                if (user == null)
                {
                    resp = new LoginResponseModel
                    {
                        Data = null,
                        Errors = Array.Empty<string>(),
                        Succeeded = false,
                        IsEmailConfirmed = false,
                        Message = "User not found",
                        SignInResult = result
                    };
                }
                else if (!user.EmailConfirmed)
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    var token = TokenUtil.GetToken(user, roles, _configuration);
                    resp = new LoginResponseModel
                    {
                        Data = token, 
                        Errors = Array.Empty<string>(), 
                        Succeeded = false, 
                        IsEmailConfirmed = false,
                        Message = "Email not confirmed", 
                        SignInResult = result
                    };
                }
                else
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    var token = TokenUtil.GetToken(user, roles, _configuration);
                    resp = new LoginResponseModel
                    {
                        Data = token, 
                        Errors = Array.Empty<string>(), 
                        Succeeded = true, 
                        IsEmailConfirmed = true,
                        Message = "", 
                        SignInResult = result
                    };
                }
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                resp = new LoginResponseModel
                {
                    Data = null, 
                    Errors = Array.Empty<string>(), 
                    Succeeded = false, 
                    IsEmailConfirmed = false,
                    Message = "User not found", 
                    SignInResult = result
                };
            }
            else
            {
                resp = new LoginResponseModel
                {
                    Data = null,
                    Errors = Array.Empty<string>(), 
                    Succeeded = false, 
                    IsEmailConfirmed = false,
                    Message = "User not found", 
                    SignInResult = result
                };
            }

            return resp;
        }
    }
}