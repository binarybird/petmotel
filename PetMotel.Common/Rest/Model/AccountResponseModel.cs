using PetMotel.Common.Rest.Entity;

namespace PetMotel.Common.Rest.Model
{
    public class AccountResponseModel
    {
        public AccountResponseModel(PetMotelUser user, bool succeeded)
        {
            Succeeded = succeeded;
            User = user;
        }

        public bool Succeeded { get; }
        public PetMotelUser User { get; }
    }
}