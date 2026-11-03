using EersteWebNetApplicatie.Models;
using EersteWebNetApplicatie.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using EersteWebNetApplicatie.DTOs;

namespace EersteWebNetApplicatie.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BlogPostController : ControllerBase
    {

        readonly BlogpostService service;

        public BlogPostController(BlogpostService service) {
            this.service = service; 
        }



        [HttpGet]
        public async Task<List<BlogPostResponseDTO>> GetBlogposts()
        {

            List<BlogPostResponseDTO> _blogposts = await service.GetBlogposts();

            return _blogposts;

        }

       
        [HttpGet("{id}")]

        public async Task<ActionResult<BlogPostResponseDTO>> GetBlogpostById(int id) {

            BlogPostResponseDTO? _blogpost = await service.GetBlogpostById(id);

            if (_blogpost == null) { return NotFound(); }
            else { return _blogpost; }

                

        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<BlogPostResponseDTO>>> DeleteById(int id) {

            List<BlogPostResponseDTO>? _blogposts = await service.DeleteById(id);


            if (_blogposts == null) { return NotFound(); } else { return _blogposts;  }

        }


        [Authorize (Roles = "Admin,Editor")]
        [HttpPost]

        public async Task<ActionResult<BlogPostResponseDTO>> CreateNewBlogpost(BlogPostRequestDTO blogpost) {




            BlogPostResponseDTO newBlogpost = await service.CreateNewBlogpost(blogpost);



          

            return CreatedAtAction(nameof(GetBlogpostById), new {id = newBlogpost.ID } , newBlogpost);
        
        }


        [Authorize (Roles = ("Admin, Editor"))]
        [HttpPut("{id}")]

        public async Task<ActionResult<BlogPostResponseDTO>> UpdateBlogpost(int id, BlogPostRequestDTO blogpost) {


            BlogPostResponseDTO? _blogpost = await service.UpdateBlogpost(id, blogpost);




            if (_blogpost == null) { return NotFound();  } else { 


                return _blogpost;
            }




        
        }







    }
}
