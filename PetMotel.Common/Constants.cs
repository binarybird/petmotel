namespace PetMotel.Common
{
    public static class Constants
    {
        public static class Config
        {
            public const string TokenSigningKey = "Tokens:SigningKey";
            public const string TokenAudience = "Tokens:Audience";
            public const string TokenIssuer = "Tokens:Issuer";
            public const string TokenLifetime = "Tokens:Lifetime";
        }
        
        public static class Roles
        {
            public const string Root = "Root";
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string Employee = "Employee";
            public const string Contributor = "Contributor";
            public const string User = "User";
        }
    }
}