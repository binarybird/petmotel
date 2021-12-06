using Microsoft.AspNetCore.Identity;

namespace PetMotel.Common.Rest.Entity
{
    public class PetMotelRole : IdentityRole
    {
        public string Description { get; set; }
    }
}