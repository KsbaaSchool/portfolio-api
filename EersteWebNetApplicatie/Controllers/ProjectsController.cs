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
        public async Task<List<Project>> getProjecten() {



            return await service.getProjecten();


        }


        

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> getProjectByID(int id) {


            Project? project = await service.getProjectByID(id);
            if (project == null) {
                return NotFound();
            }
            else
            {
                return project;
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Project>>> deleteProjectByID(int id) {


            List<Project>? _projects = await service.deleteProjectByID(id);

            if (_projects == null) { return NotFound();  } else {
                return _projects;

            } }




        [HttpPost]

        public async Task<ActionResult<Project>> createnewProject(ProjectRequestDTO project)

        {

            Project newproject = await service.createnewProject(project);

            return CreatedAtAction(nameof(getProjectByID), new { id = newproject.ID } , newproject);


        }
    

        [HttpPut("{id}")]

        public async Task<ActionResult<Project>> updateProjectbyID(int id, ProjectRequestDTO project) {

            Project? _project = await service.updateProjectbyID(id, project);



            if (_project == null) { return NotFound(); } else
            {
                return _project;

            }

        }





    }
}
