using System.ComponentModel.DataAnnotations;

namespace EersteWebNetApplicatie.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public String email { get; set; } = "";

        [Required]
        public String wachtwoord { get; set; } = "";

    }
}
