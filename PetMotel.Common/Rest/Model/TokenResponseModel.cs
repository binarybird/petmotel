using Microsoft.AspNetCore.Identity;
using PetMotel.Common.Rest.Entity;

namespace PetMotel.Common.Rest.Model
{
    public class TokenResponseModel
    {
        public TokenResponseModel(string refreshedToken, bool isValid, bool succeeded)
        {
            RefreshedToken = refreshedToken;
            IsValid = isValid;
            Succeeded = succeeded;
        }
        public string RefreshedToken { get; }
        public bool IsValid { get; }
        public bool Succeeded { get; }
    }
}