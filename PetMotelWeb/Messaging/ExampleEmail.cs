using Common;

namespace PetMotelWeb.Messaging
{
    public class ExampleEmail : IExampleEmail
    {
        public ExampleEmail(string email)
        {
            Email = email;
        }

        public ExampleEmail()
        {
            
        }

        public string Email { get; set; }
    }
}