using Microsoft.AspNetCore.Identity;
using PetMotel.Common.Rest.Entity;

namespace PetMotel.Common.Rest.Model
{
    public class RegisterResponseModel
    {
        public RegisterResponseModel(PetMotelUser user, IdentityResult result, bool isLoggedIn, bool isConfirmationRequired)
        {
            User = user;
            Result = result;
            IsLoggedIn = isLoggedIn;
            IsConfirmationRequired = isConfirmationRequired;
        }

        public PetMotelUser User { get; }
        
        public IdentityResult Result { get; }
        
        public bool IsLoggedIn { get; }
        
        public bool IsConfirmationRequired { get; }
    }
}