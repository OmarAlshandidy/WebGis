using System.ComponentModel.DataAnnotations;

namespace Gis.PL.Dtos.Auth
{
    public class SignUpDto
    {
        [Required(ErrorMessage ="UserName Is Required !! ")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FirstName Is Required !! ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Is Required !! ")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Is Required !! ")]
        [EmailAddress]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required !! ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConformPassword Is Required !! ")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Confirm Password dose not match the password !! ")]
        public string ConformPassword { get; set; }
        [Required(ErrorMessage = "IsAgreed Is Required !! ")]

        public bool IsAgreed { get; set; }
    }
}
