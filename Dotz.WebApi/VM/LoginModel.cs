using System.ComponentModel.DataAnnotations;

namespace Dotz.WebApi.VM
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
