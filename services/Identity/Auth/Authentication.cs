using System.Threading.Tasks;
using Identity.Entity;
using Microsoft.AspNetCore.Identity;

namespace Identity.Auth
{
    public class Authentication : IAuthentication
    {
        readonly UserManager<ApplicationUser> _userManager;

        public Authentication(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<User> CreateUser(string email, string password)
        {
            var applicationUser = new ApplicationUser()
            {
                Email = email,
                UserName = email,
            };

            
            var result = await _userManager.CreateAsync(applicationUser, password);
            if (!result.Succeeded)
            {
                return null;
            }
            
            var loaded = await _userManager.FindByNameAsync(email);
            return DTOMapper.Map<User>(loaded);
        }
        
        public async Task<User> UpdatePassword(string email, string oldPassword, string newPassword)
        {
            var loaded1 = await _userManager.FindByNameAsync(email);
            if (loaded1 == null)
            {
                return null;
            }
            
            var result = await _userManager.ChangePasswordAsync(loaded1, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                return null;
            }
            
            var loaded2 = await _userManager.FindByNameAsync(email);
            return DTOMapper.Map<User>(loaded2);
        }
        
        public async Task<bool> CheckPassword(string email, string password)
        {
            var loaded1 = await _userManager.FindByNameAsync(email);
            if (loaded1 == null)
            {
                return false;
            }
            
            return await _userManager.CheckPasswordAsync(loaded1, password);
        }
    }

    public interface IAuthentication
    {
    }
}