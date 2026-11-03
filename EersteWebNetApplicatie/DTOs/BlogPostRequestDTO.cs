using System.ComponentModel.DataAnnotations;

namespace EersteWebNetApplicatie.DTOs
{
    public class BlogPostRequestDTO
    {


        [Required]
        public String titel { get; set; } = "";

        [Required]
        public String inhoud { get; set; } = "";

        [Required]
        public DateTime? publicatieDatum { get; set; }



     
    }
}
