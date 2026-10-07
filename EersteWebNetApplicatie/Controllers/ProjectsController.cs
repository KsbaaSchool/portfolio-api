using EersteWebNetApplicatie.Models;
using EersteWebNetApplicatie.Services;
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
        public async Task<List<ProjectResponseDTO>> getProjecten() {



            return await service.getProjecten();


        }       


        

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseDTO>> getProjectByID(int id) {


            ProjectResponseDTO? project = await service.getProjectByID(id);
            if (project == null) {
                return NotFound();
            }
            else
            {
                return project;
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ProjectResponseDTO>>> deleteProjectByID(int id) {


            List<ProjectResponseDTO>? _projects = await service.deleteProjectByID(id);

            if (_projects == null) { return NotFound();  } else {
                return _projects;

            } }




        [HttpPost]

        public async Task<ActionResult<ProjectResponseDTO>> createnewProject(ProjectRequestDTO project)

        {

            ProjectResponseDTO newproject = await service.createnewProject(project);

            return CreatedAtAction(nameof(getProjectByID), new { id = newproject.ID } , newproject);


        }
    

        [HttpPut("{id}")]

        public async Task<ActionResult<ProjectResponseDTO>> updateProjectbyID(int id, ProjectRequestDTO project) {

            ProjectResponseDTO? _project = await service.updateProjectbyID(id, project);



            if (_project == null) { return NotFound(); } else
            {
                return _project;

            }

        }





    }
}
