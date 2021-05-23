using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PetMotel.Common.Rest.Entity;
using PetMotel.Common.Rest.Model;
using PetMotel.Identity.Data;

namespace PetMotel.Identity.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly SignInManager<PetMotelUser> _signInManager;
        private readonly UserManager<PetMotelUser> _userManager;
        private readonly ILogger<RegisterRequestModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly PetMotelIdentityContext _context;
        private RoleManager<PetMotelRole> _roleManager;
        
        public RegisterController(
            PetMotelIdentityContext context,
            UserManager<PetMotelUser> userManager,
            SignInManager<PetMotelUser> signInManager,
            ILogger<RegisterRequestModel> logger,
            IEmailSender emailSender,
            RoleManager<PetMotelRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult<RegisterRequestModel>> PostRegisterModel(RegisterRequestModel petMotelRegisterModel)
        {
            var ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //TODO - clean and validate here

            var user = new PetMotelUser { UserName = petMotelRegisterModel.Email, Email = petMotelRegisterModel.Email };
            var result = await _userManager.CreateAsync(user, petMotelRegisterModel.Password);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                //TODO: fix this shit
                string callbackUrl = $"www.petmotel.org/user/{user.Id}/register/{code}";

                await _emailSender.SendEmailAsync(petMotelRegisterModel.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = petMotelRegisterModel.Email, returnUrl = "returnUrl" });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("returnUrl");
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }


            // If we got this far, something failed, redisplay form
            return null;
        }
    }
}

