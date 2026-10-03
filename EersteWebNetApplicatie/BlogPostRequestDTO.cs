namespace EersteWebNetApplicatie
{
    public class BlogPostRequestDTO
    {



        public String titel { get; set; }

        public String inhoud { get; set; }

        public DateTime publicatieDatum { get; set; }



        public BlogPostRequestDTO(String titel, String inhoud, DateTime publicatieDatum)
        {
            this.titel = titel;
            this.inhoud = inhoud;
            this.publicatieDatum = publicatieDatum;
        }
    }
}
