using System.ComponentModel.DataAnnotations;

namespace EersteWebNetApplicatie.DTOs
{
    public class BlogPostRequestDTO
    {


        [Required]
        public String titel { get; set; }

        [Required]
        public String inhoud { get; set; }

        [Required]
        public DateTime? publicatieDatum { get; set; }



        public BlogPostRequestDTO(String titel, String inhoud, DateTime? publicatieDatum)
        {
            this.titel = titel;
            this.inhoud = inhoud;
            this.publicatieDatum = publicatieDatum;
        }
    }
}
