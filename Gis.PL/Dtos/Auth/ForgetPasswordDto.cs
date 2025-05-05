using System.ComponentModel.DataAnnotations;

namespace Gis.PL.Dtos.Auth
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email Is Required !! ")]
        [EmailAddress]

        public string Email { get; set; }
    }
}
