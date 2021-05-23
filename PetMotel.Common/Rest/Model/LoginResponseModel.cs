using PetMotel.Common.Rest.Entity;

namespace PetMotel.Common.Rest.Model
{
    public class LoginResponseModel
    {
        public LoginResponseModel(string jwt, bool succeeded, bool isLockedOut, bool isNotAllowed, bool requiresTwoFactor)
        {
            JWT = jwt;
            Succeeded = succeeded;
            IsLockedOut = isLockedOut;
            IsNotAllowed = isNotAllowed;
            RequiresTwoFactor = requiresTwoFactor;
        }

        public string JWT { get; }
        
        /// <summary>
        /// Returns a flag indication whether the sign-in was successful.
        /// </summary>
        /// <value>True if the sign-in was successful, otherwise false.</value>
        public bool Succeeded { get; }

        /// <summary>
        /// Returns a flag indication whether the user attempting to sign-in is locked out.
        /// </summary>
        /// <value>True if the user attempting to sign-in is locked out, otherwise false.</value>
        public bool IsLockedOut { get; }

        /// <summary>
        /// Returns a flag indication whether the user attempting to sign-in is not allowed to sign-in.
        /// </summary>
        /// <value>True if the user attempting to sign-in is not allowed to sign-in, otherwise false.</value>
        public bool IsNotAllowed { get; }

        /// <summary>
        /// Returns a flag indication whether the user attempting to sign-in requires two factor authentication.
        /// </summary>
        /// <value>True if the user attempting to sign-in requires two factor authentication, otherwise false.</value>
        public bool RequiresTwoFactor { get; }
    }
}