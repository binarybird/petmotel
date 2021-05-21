using Microsoft.AspNetCore.Identity;

namespace PetMotel.Identity.Entity
{
    
    public static class Roles
    {
        public const string Root = "Root";
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Employee = "Employee";
        public const string Contributor = "Contributor";
        public const string User = "User";
    }
    public class PetMotelRole : IdentityRole
    {
        public string Description { get; set; }
    }
}