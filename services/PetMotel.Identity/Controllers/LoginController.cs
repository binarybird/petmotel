using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public LoginController(SignInManager<PetMotelUser> signInManager,
            ILogger<PetMotelLoginModel> logger,
            UserManager<PetMotelUser> userManager,
            PetMotelIdentityContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: api/Login
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetMotelLoginModel>>> GetLoginModel()
        {
            return await _context.LoginModel.ToListAsync();
        }

        // GET: api/Login/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PetMotelLoginModel>> GetLoginModel(int id)
        {
            var loginModel = await _context.LoginModel.FindAsync(id);

            if (loginModel == null)
            {
                return NotFound();
            }

            return loginModel;
        }

        // PUT: api/Login/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutLoginModel(int id, LoginModel loginModel)
        //{
        //    if (id != loginModel.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(loginModel).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LoginModelExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PetMotelLoginModel>> PostLoginModel(PetMotelLoginModel petMotelLoginModel)
        {
            var ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(petMotelLoginModel.Email, petMotelLoginModel.Password, petMotelLoginModel.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return CreatedAtAction("GetLoginModel", new { id = petMotelLoginModel.Id }, petMotelLoginModel);
            }
            if (result.RequiresTwoFactor)
            {
                //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                //return RedirectToPage("./Lockout");
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //return Page();
            }


            //    _context.LoginModel.Add(loginModel);
            //    await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoginModel", new { id = petMotelLoginModel.Id }, petMotelLoginModel);
        }

        // DELETE: api/Login/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoginModel(int id)
        {
            var loginModel = await _context.LoginModel.FindAsync(id);
            if (loginModel == null)
            {
                return NotFound();
            }

            _context.LoginModel.Remove(loginModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // private bool LoginModelExists(int id)
        // {
        //     return _context.LoginModel.Any(e => e.Id == id);
        // }
    }
}
