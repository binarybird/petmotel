using Microsoft.AspNetCore.Identity;
using PetMotel.Common.Rest.Entity;

namespace PetMotel.Common.Rest.Model
{
    public class TokenResponseModel : IResponse<string>
    {
        public bool IsValid { get; }
        public string Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}