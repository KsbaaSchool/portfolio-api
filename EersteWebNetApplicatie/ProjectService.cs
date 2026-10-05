using Microsoft.AspNetCore.Mvc;

namespace EersteWebNetApplicatie
{
    public class ProjectService
    {
        private static List<Project> _projects = new List<Project> {

            new Project("Project 1", 324, "Beschrijving 1", "Categorie 1", "https://github.com/project1", new DateTime(2026, 10, 1)),

                new Project("Project 2", 112, "Beschrijving 1", "Categorie 1", "https://github.com/project1", new DateTime(2026, 10, 1))

            };



        public List<Project> getProjecten()
        {



            return _projects;


        }




            public Project? getProjectByID(int id)
            {


                Project? project = _projects.FirstOrDefault(p => p.ID == id);

            return project;
        }

        public List<Project>? deleteProjectByID(int id)
        {

            Project? _project = _projects.FirstOrDefault(project => id == project.ID);

            if (_projects.Remove(_project)) { return _projects; }
            else
            {
                return null;

            }
        }





        public Project? createnewProject(ProjectRequestDTO project)

        {
            Random random = new Random();

            int uniekgetal;


            do
            {
                uniekgetal = random.Next(0, 1000);
            }
            while (_projects.Any(p => p.ID == uniekgetal));

            int id = uniekgetal;


            Project newproject = new Project(
                project.titel,
                id,
                project.beschrijving,
                project.Categorie,
                project.GitHubUrl,
                project.Datum
            );


            _projects.Add(newproject);


            return newproject;


        }




        public Project? updateProjectbyID(int id, ProjectRequestDTO project)
        {

            Project? _project = _projects.FirstOrDefault(p => p.ID == id);

            if (_project == null) { return null; }
            else
            {
                _project.titel = project.titel;
                _project.beschrijving = project.beschrijving;
                _project.Categorie = project.Categorie;
                _project.GitHubUrl = project.GitHubUrl;
                _project.Datum = project.Datum;

                return _project;



            }

        }









    }
}
