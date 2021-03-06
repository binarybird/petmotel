using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PetMotel.Common;
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

        public RegisterController(
            UserManager<PetMotelUser> userManager,
            SignInManager<PetMotelUser> signInManager,
            ILogger<RegisterRequestModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<RegisterResponseModel> ValidateEmail([FromQuery] string token, [FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new RegisterResponseModel
                {
                    Data = null,
                    Errors = Array.Empty<string>(),
                    IsConfirmationRequired = false,
                    IsLoggedIn = false,
                    Message = "",
                    Result = IdentityResult.Failed(new IdentityError {Description = "Unable to validate email code"}),
                    Succeeded = false
                };
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return new RegisterResponseModel
            {
                Data = user,
                Errors = Array.Empty<string>(),
                IsConfirmationRequired = false,
                IsLoggedIn = false,
                Message = "",
                Result = result,
                Succeeded = true
            };
        }

        [HttpPost]
        public async Task<RegisterResponseModel> PostRegisterModel(RegisterRequestModel petMotelRegisterModel)
        {
            var user = new PetMotelUser {UserName = petMotelRegisterModel.Email, Email = petMotelRegisterModel.Email};
            var result = await _userManager.CreateAsync(user, petMotelRegisterModel.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                IdentityResult addToRoleAsync = await _userManager.AddToRoleAsync(user, Constants.Roles.User);
                if (!addToRoleAsync.Succeeded)
                {
                    Task<IdentityResult> deleteAsync = _userManager.DeleteAsync(user);
                    return new RegisterResponseModel
                    {
                        Data = null,
                        Errors = Array.Empty<string>(),
                        IsConfirmationRequired = false,
                        IsLoggedIn = false,
                        Message = "Unable to assign role",
                        Result = result,
                        Succeeded = false
                    };
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                
                string callbackUrl = $"www.petmotel.org/Register?token={code}&email={petMotelRegisterModel.Email}";

                await _emailSender.SendEmailAsync(petMotelRegisterModel.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return new RegisterResponseModel
                    {
                        Data = user,
                        Errors = Array.Empty<string>(),
                        IsConfirmationRequired = true,
                        IsLoggedIn = false,
                        Message = "",
                        Result = result,
                        Succeeded = true
                    };
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new RegisterResponseModel
                    {
                        Data = user,
                        Errors = Array.Empty<string>(),
                        IsConfirmationRequired = false,
                        IsLoggedIn = false,
                        Message = "",
                        Result = result,
                        Succeeded = true
                    };
                }
            }

            return new RegisterResponseModel
            {
                Data = null,
                Errors = Array.Empty<string>(),
                IsConfirmationRequired = false,
                IsLoggedIn = false,
                Message = "",
                Result = result,
                Succeeded = false
            };
        }
    }
}