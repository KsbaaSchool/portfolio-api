using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace EersteWebNetApplicatie.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BlogPostController : ControllerBase
    {

        public static List<Blogpost> _blogposts = new List<Blogpost> {
                new Blogpost (1, "Titel 1", "Inhoud 1", new DateTime(2026, 10, 1)),
                new Blogpost (2, "Titel 2", "Inhoud 2", new DateTime(2026, 10, 1))};



        [HttpGet]
        public List<Blogpost> getBlogposts()
        {


            return _blogposts;

        }


        [HttpGet("{id}")]

        public ActionResult<Blogpost> getBlogpostByID(int id) {

            Blogpost? _blogpost = _blogposts.FirstOrDefault(blogpost => id == blogpost.ID);

            if (_blogpost == null) { return NotFound(); }
            else { return _blogpost; }

                
        
        }




    }
}
