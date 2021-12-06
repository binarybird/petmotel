using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IEmailSender _emailSender;

        public AccountController(SignInManager<PetMotelUser> signInManager,
            ILogger<AccountController> logger,
            UserManager<PetMotelUser> userManager,
            PetMotelIdentityContext context,
            IConfiguration configuration,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        [HttpPatch]
        public async Task<AccountResponseModel> ResetPassword([FromQuery] string newPass, [FromQuery] string resetToken,
            [FromQuery] string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
            {
                return new AccountResponseModel {
                    Data = null, 
                    Errors = Array.Empty<string>(), 
                    Message = "Unable to find user", 
                    Succeeded = false};
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPass);

            string[] errs = result.Errors.Select(s => s.Description).ToArray();

            return new AccountResponseModel {Data = user, Errors = errs, Message = "", Succeeded = result.Succeeded};
        }

        [HttpPut]
        public async Task<AccountResponseModel> ResetPassword([FromQuery] string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
            {
                return null;
            }

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

            string callbackUrl = $"www.petmotel.org/Reset?token={resetToken}&email={email}";

            await _emailSender.SendEmailAsync(email, "Password reset request",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            throw new NotImplementedException();
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
                return new AccountResponseModel
                    {Data = null, Errors = Array.Empty<string>(), Message = "User not found", Succeeded = false};
            }

            IdentityResult result = await _userManager.UpdateAsync(update);
            string[] errs = result.Errors.Select(s => s.Description).ToArray();

            return new AccountResponseModel
                {Data = update, Errors = errs, Message = "", Succeeded = result.Succeeded};
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
                return new AccountResponseModel
                    {Data = null, Errors = Array.Empty<string>(), Message = "User not found", Succeeded = false};
            }

            return new AccountResponseModel
                {Data = user, Errors = Array.Empty<string>(), Message = "", Succeeded = false};
        }

        [HttpDelete]
        public async Task<AccountResponseModel> Delete()
        {
            throw new NotImplementedException();
        }
    }
}