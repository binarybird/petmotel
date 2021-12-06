namespace PetMotel.Common.Rest.Model
{
    public class TokenRequestModel
    {
        public TokenRequestModel(string token)
        {
            Token = token;
        }
        
        public string Token { get; }
    }
}