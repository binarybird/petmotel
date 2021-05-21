using System.ComponentModel.DataAnnotations;

namespace PetMotel.Common.Rest.Model
{
    public class PetMotelLoginModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
