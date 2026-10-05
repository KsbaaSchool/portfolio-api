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


        [HttpDelete("{id}")]
        public ActionResult<List<Blogpost>> deleteById(int id) {

            Blogpost? _blogpost = _blogposts.FirstOrDefault(b => b.ID == id);

            if (_blogposts.Remove(_blogpost)) { return _blogposts; } else { return NotFound(); }

        }


        [HttpPost]

        public ActionResult<Blogpost> createnewBlogpost(BlogPostRequestDTO blogpost) {


            Random random = new Random();
            int uniekgetal;

            do { uniekgetal = random.Next(0, 1001); }
            while (_blogposts.Any(bp => bp.ID == uniekgetal));


            Blogpost newBlogpost = new Blogpost(uniekgetal, blogpost.titel, blogpost.inhoud, blogpost.publicatieDatum);


            _blogposts.Add(newBlogpost);

            return CreatedAtAction(nameof(getBlogpostByID), new {id = uniekgetal } , newBlogpost);
        
        }


        [HttpPut("{id}")]

        public ActionResult<Blogpost> updateBlogpost(int id, BlogPostRequestDTO blogpost) {

            Blogpost? _blogpost = _blogposts.FirstOrDefault(bg => bg.ID == id);




            if (_blogpost == null) { return NotFound();  } else { 

                _blogpost.titel = blogpost.titel;
                _blogpost.inhoud = blogpost.inhoud;
                _blogpost.publicatieDatum = blogpost.publicatieDatum;


                return _blogpost;
            }




        
        }







    }
}
