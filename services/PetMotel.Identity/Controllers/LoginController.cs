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
using PetMotel.Common.Rest.Model;
using PetMotel.Identity.Data;
using PetMotel.Identity.Entity;

namespace PetMotel.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly PetMotelIdentityContext _context;
        private readonly UserManager<PetMotelUser> _userManager;
        private readonly SignInManager<PetMotelUser> _signInManager;
        private readonly ILogger<PetMotelLoginModel> _logger;
        private readonly IConfiguration _configuration;

        public LoginController(SignInManager<PetMotelUser> signInManager,
            ILogger<PetMotelLoginModel> logger,
            UserManager<PetMotelUser> userManager,
            PetMotelIdentityContext context,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // POST: api/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PetMotelLoginModel>> PostLoginModel(PetMotelLoginModel petMotelLoginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(
                petMotelLoginModel.Email,
                petMotelLoginModel.Password,
                petMotelLoginModel.RememberMe,
                lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                var user = await _userManager.FindByNameAsync(petMotelLoginModel.Email);
                var token = GetToken(user);
                return CreatedAtAction("GetLoginModel", new {id = petMotelLoginModel.Id}, petMotelLoginModel);
            }
            
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return CreatedAtAction("GetLoginModel", new {id = petMotelLoginModel.Id}, petMotelLoginModel);
            }
            
            return CreatedAtAction("GetLoginModel", new {id = petMotelLoginModel.Id}, petMotelLoginModel);
        }
        
        // [Authorize]  
        // [HttpPost]  
        // // [Route("refreshtoken")]  
        // public async Task<IActionResult> RefreshToken()  
        // {  
        //     var user = await _userManager.FindByNameAsync(  
        //         User.Identity.Name ??  
        //         User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()  
        //     );  
        //     return Ok(GetToken(user));  
        //
        // }  

        //https://dejanstojanovic.net/aspnet/2018/june/token-based-authentication-in-aspnet-core-part-2/
        private String GetToken(IdentityUser user)  
        {  
            var utcNow = DateTime.UtcNow;  
  
            var claims = new Claim[]  
            {  
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),  
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())  
            };  
  
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<String>("Tokens:Key")));  
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);  
            var jwt = new JwtSecurityToken(  
                signingCredentials: signingCredentials,  
                claims: claims,  
                notBefore: utcNow,  
                expires: utcNow.AddSeconds(_configuration.GetValue<int>("Tokens:Lifetime")),  
                audience: _configuration.GetValue<String>("Tokens:Audience"),  
                issuer: _configuration.GetValue<String>("Tokens:Issuer")  
            );  
  
            return new JwtSecurityTokenHandler().WriteToken(jwt);  
  
        }  
    }
}