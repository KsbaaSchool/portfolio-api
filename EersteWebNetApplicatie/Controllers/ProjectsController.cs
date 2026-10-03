using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace EersteWebNetApplicatie.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {

        
        public static List<Project> _projects = new List<Project> {

            new Project("Project 1", 324, "Beschrijving 1", "Categorie 1", "https://github.com/project1", new DateTime(2026, 10, 1)),

                new Project("Project 2", 112, "Beschrijving 1", "Categorie 1", "https://github.com/project1", new DateTime(2026, 10, 1))

            };


        [HttpGet]
        public List<Project> getProjecten() {



            return _projects;


        }


        

        [HttpGet("{id}")]
        public ActionResult<Project> getProjectByID(int id) {

        
           Project? project = _projects.FirstOrDefault(p => p.ID == id);
            if (project == null) {
                return NotFound();
            }
            else
            {
                return project;
            }

        }

        [HttpDelete("{id}")]
        public ActionResult<List<Project>> deleteProjectByID(int id) {

            Project? _project = _projects.FirstOrDefault(project => id == project.ID);

            if (_projects.Remove(_project)) { return _projects; } else { return NotFound();

            } }




        [HttpPost]




            public List<Project> createnewProject(String titel, String beschrijving, String Categorie, String Githuburl, DateTime Datum)
           
            {
            Random random = new Random();

            int uniekgetal;


            do
            {
                uniekgetal = random.Next(0, 1000);
            }
            while (_projects.Any(p => p.ID == uniekgetal));

            int id = uniekgetal; 




            Project newproject = new Project(titel,id,beschrijving,Categorie,Githuburl,Datum);

            _projects.Add(newproject);


            return _projects;


        }








    }
}
