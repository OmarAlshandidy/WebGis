using System.ComponentModel.DataAnnotations;

namespace Gis.PL.Dtos.Auth
{
    public class UserToReturnDto
    {

        public string Id { get; set; }
        [Required(ErrorMessage = "UserName Is Required !! ")]

        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName Is Required !! ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Is Required !! ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Is Required !! ")]
        public string Email { get; set; }

        public IEnumerable<string> ? Roles { get; set; }
    }
}
