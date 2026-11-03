using EersteWebNetApplicatie.Models;
using EersteWebNetApplicatie.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using EersteWebNetApplicatie.DTOs;
using System.ComponentModel.DataAnnotations;


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
        public async Task<List<ProjectResponseDTO>> GetProjecten() {



            return await service.GetProjecten();


        }       


        

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseDTO>> GetProjectById(int id) {


            ProjectResponseDTO? project = await service.GetProjectById(id);
            if (project == null) {
                return NotFound();
            }
            else
            {
                return project;
            }

        }
        
        
        [Authorize(Roles = "Admin,Editor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ProjectResponseDTO>>> DeleteProjectById(int id) {


            List<ProjectResponseDTO>? _projects = await service.DeleteProjectById(id);

            if (_projects == null) { return NotFound();  } else {
                return _projects;

            } }




        
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]

        public async Task<ActionResult<ProjectResponseDTO>> CreateNewProject(ProjectRequestDTO project)

        {

            ProjectResponseDTO newproject = await service.CreateNewProject(project);

            return CreatedAtAction(nameof(GetProjectById), new { id = newproject.ID } , newproject);


        }

        
        [Authorize(Roles = "Admin,Editor")]
        [HttpPut("{id}")]

        public async Task<ActionResult<ProjectResponseDTO>> UpdateProjectById(int id, ProjectRequestDTO project) {

            ProjectResponseDTO? _project = await service.UpdateProjectById(id, project);



            if (_project == null) { return NotFound(); } else
            {
                return _project;

            }

        }





    }
}
