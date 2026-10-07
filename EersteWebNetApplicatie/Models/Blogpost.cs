namespace EersteWebNetApplicatie.Models
{
    public class Blogpost
    {

       public int ID { get; set; }

        public String titel { get; set; }

        public String inhoud { get; set; }

        public DateTime publicatieDatum { get; set; }



        public Blogpost(int ID, String titel, String inhoud, DateTime publicatieDatum)
        {
            this.ID = ID;
            this.titel = titel;
            this.inhoud = inhoud;
            this.publicatieDatum = publicatieDatum;
        }




    }
}
