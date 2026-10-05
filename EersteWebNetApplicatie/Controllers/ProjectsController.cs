using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace EersteWebNetApplicatie.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProjectsController : ControllerBase
    {

      readonly ProjectService service;

        public ProjectsController(ProjectService service) {

            this.service = service;
        
        
        }

        



        [HttpGet]
        public List<Project> getProjecten() {



            return service.getProjecten();


        }


        

        [HttpGet("{id}")]
        public ActionResult<Project> getProjectByID(int id) {


            Project? project = service.getProjectByID(id);
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


            List<Project>?   _projects = service.deleteProjectByID(id);

            if (_projects == null) { return NotFound();  } else {
                return _projects;

            } }




        [HttpPost]

        public ActionResult<Project> createnewProject(ProjectRequestDTO project)

        {

            Project? newproject = service.createnewProject(project);

            return CreatedAtAction(nameof(getProjectByID), new { id = newproject.ID } , newproject);


        }


        [HttpPut("{id}")]

        public ActionResult<Project> updateProjectbyID(int id, ProjectRequestDTO project) {

            Project? _project = service.updateProjectbyID(id, project);



            if (_project == null) { return NotFound(); } else
            {
                return _project;

            }

        }





    }
}
