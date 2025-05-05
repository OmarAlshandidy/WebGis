using System.ComponentModel.DataAnnotations;

namespace Gis.PL.Dtos.Auth
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Email Is Required !! ")]
        [EmailAddress(ErrorMessage = "Invalid Email !!")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required !! ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RemmeberMe { get; set; }
    }
}
