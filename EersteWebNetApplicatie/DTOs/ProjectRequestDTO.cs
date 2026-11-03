using System.ComponentModel.DataAnnotations;

namespace EersteWebNetApplicatie.DTOs
{
    public class ProjectRequestDTO
    {

        [Required]
        public String titel { get; set; } = "";

        [Required]
        public String beschrijving { get; set; } = "";

        [Required]
        public String Categorie { get; set; } = "";

        [Required]
        public String GitHubUrl { get; set; } = "";

        [Required]
        public DateTime? Datum { get; set; } 






    }
}
