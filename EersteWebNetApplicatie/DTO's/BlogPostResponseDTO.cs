namespace EersteWebNetApplicatie.DTO_s
{
    public class BlogPostResponseDTO
    {
        public int ID { get; set; }

        public String titel { get; set; }

        public String inhoud { get; set; }

        public DateTime publicatieDatum { get; set; }



        public BlogPostResponseDTO(int ID, String titel, String inhoud, DateTime publicatieDatum)
        {
            this.ID = ID;
            this.titel = titel;
            this.inhoud = inhoud;
            this.publicatieDatum = publicatieDatum;
        }




    }
}
