namespace EersteWebNetApplicatie
{
    public class ProjectRequestDTO
    {

        public String titel { get; set; }

        public String beschrijving { get; set; }

        public String Categorie { get; set; }

        public String GitHubUrl { get; set; }

        public DateTime Datum { get; set; }



        public ProjectRequestDTO(String titel, String beschrijving, String Categorie, String GitHubUrl, DateTime Datum)
        {
            this.titel = titel;
            this.beschrijving = beschrijving;
            this.Categorie = Categorie;
            this.GitHubUrl = GitHubUrl;
            this.Datum = Datum;
        }


    }
}
