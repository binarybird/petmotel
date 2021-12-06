using PetMotel.Common.Rest.Entity;

namespace PetMotel.Common.Rest.Model
{
    public class UsersResponseModel : IResponse<PetMotelUser>
    {
        public PetMotelUser Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}