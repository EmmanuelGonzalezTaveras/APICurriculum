using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Cuentas
{
    public class UserInfo
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
