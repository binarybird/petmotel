using Common;

namespace PetMotelWeb.Messaging
{
    public class ExampleEmail : IExampleEmail
    {
        private readonly string _email;
        public ExampleEmail(string email)
        {
            _email = email;
        }
        
        public string GetEmail()
        {
            return _email;
        }
    }
}