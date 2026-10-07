namespace EersteWebNetApplicatie.Models
{
    public class Project
    {

        public String titel { get; set; }
        public int ID { get; set; }

        public String beschrijving { get; set; }

        public String Categorie { get; set; }

        public String GitHubUrl { get; set; }

        public DateTime Datum { get; set; }



       public Project(String titel, int ID, String beschrijving, String Categorie, String GitHubUrl, DateTime Datum)
        {
            this.titel = titel;
            this.ID = ID;
            this.beschrijving = beschrijving;
            this.Categorie = Categorie;
            this.GitHubUrl = GitHubUrl;
            this.Datum = Datum;
        }


    }
}
