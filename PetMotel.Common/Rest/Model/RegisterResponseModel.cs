using Microsoft.AspNetCore.Identity;
using PetMotel.Common.Rest.Entity;

namespace PetMotel.Common.Rest.Model
{
    public class RegisterResponseModel : IResponse<PetMotelUser>
    {
        public PetMotelUser Data { get; set; }
        
        public bool Succeeded { get; set; }
        
        public string Message { get; set; }
        
        public string[] Errors { get; set; }

        public IdentityResult Result { get; set; }

        public bool IsLoggedIn { get; set; }

        public bool IsConfirmationRequired { get; set; }
    }
}