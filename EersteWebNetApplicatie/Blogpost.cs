namespace EersteWebNetApplicatie
{
    public class Blogpost
    {

       public int ID { get; }

        public String titel { get; set; }

        public String inhoud { get; set; }

        public DateTime publicatieDatum { get; set; }



        public Blogpost(int id, String titel, String inhoud, DateTime publicatieDatum)
        {
            this.ID = id;
            this.titel = titel;
            this.inhoud = inhoud;
            this.publicatieDatum = publicatieDatum;
        }




    }
}
