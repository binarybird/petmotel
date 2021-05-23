using Microsoft.AspNetCore.Identity;

namespace PetMotel.Common.Rest.Entity
{
    // Add profile data for application users by adding properties to the PetMotelUser class
    public class PetMotelUser : IdentityUser
    {
        public string PublicName { get; set; }
    }
}
