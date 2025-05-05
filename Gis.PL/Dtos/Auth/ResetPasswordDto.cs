using System.ComponentModel.DataAnnotations;

namespace Gis.PL.Dtos.Auth
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConformPassword Is Required !! ")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password dose not match the password !! ")]
        public string ConformPassword { get; set; }
    }

}
