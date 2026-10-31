using System.ComponentModel.DataAnnotations;

namespace EersteWebNetApplicatie.DTOs
{
    public class RegisterDTO
    {


        [Required]
        [EmailAddress]
        public String email { get; set; } = "";

        [Required]
        public String wachtwoord { get; set; } = "";

    }
}

