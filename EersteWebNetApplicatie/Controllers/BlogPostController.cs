using EersteWebNetApplicatie.DTO_s;
using EersteWebNetApplicatie.Models;
using EersteWebNetApplicatie.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

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
        public async Task<List<BlogPostResponseDTO>> getBlogposts()
        {

            List<BlogPostResponseDTO> _blogposts = await service.getBlogposts();

            return _blogposts;

        }


        [HttpGet("{id}")]

        public async Task<ActionResult<BlogPostResponseDTO>> getBlogpostByID(int id) {

            BlogPostResponseDTO? _blogpost = await service.getBlogpostByID(id);

            if (_blogpost == null) { return NotFound(); }
            else { return _blogpost; }



        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<List<BlogPostResponseDTO>>> deleteById(int id) {

            List<BlogPostResponseDTO>? _blogposts = await service.deleteById(id);


            if (_blogposts == null) { return NotFound(); } else { return _blogposts;  }

        }


        [HttpPost]

        public async Task<ActionResult<BlogPostResponseDTO>> createnewBlogpost(BlogPostRequestDTO blogpost) {




            BlogPostResponseDTO newBlogpost = await service.createnewBlogpost(blogpost);



          

            return CreatedAtAction(nameof(getBlogpostByID), new {id = newBlogpost.ID } , newBlogpost);
        
        }


        [HttpPut("{id}")]

        public async Task<ActionResult<BlogPostResponseDTO>> updateBlogpost(int id, BlogPostRequestDTO blogpost) {


            BlogPostResponseDTO? _blogpost = await service.updateBlogpost(id, blogpost);




            if (_blogpost == null) { return NotFound();  } else { 


                return _blogpost;
            }




        
        }







    }
}
