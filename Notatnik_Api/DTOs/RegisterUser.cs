using System.ComponentModel.DataAnnotations;

namespace Notatnik_Api.DTOs
{
    public class RegisterUser
    {
        [Required]
        public string Email { get; set; } 
        [Required]
        public string Password { get; set; }
    }
}
