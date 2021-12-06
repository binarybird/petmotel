using Microsoft.AspNetCore.Identity;
using PetMotel.Common.Rest.Entity;

namespace PetMotel.Common.Rest.Model
{
    public class LoginResponseModel : IResponse<string>
    {
        public SignInResult SignInResult { get; set; }
        public string Data { get; set; }
        
        public bool IsEmailConfirmed { get; set; }

        public bool Succeeded { get; set; }
        
        public string Message { get; set; }
        
        public string[] Errors { get; set; }
    }
}