using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace EersteWebNetApplicatie
{
    public class BlogpostService
    {
    
        private static List<Blogpost> _blogposts = new List<Blogpost> {
                new Blogpost (1, "Titel 1", "Inhoud 1", new DateTime(2026, 10, 1)),
                new Blogpost (2, "Titel 2", "Inhoud 2", new DateTime(2026, 10, 1))};



        public List<Blogpost> getBlogposts()
        {


            return _blogposts;

        }


        public Blogpost? getBlogpostByID(int id)
        {

            Blogpost? _blogpost = _blogposts.FirstOrDefault(blogpost => id == blogpost.ID);

            if (_blogpost == null) { return null; }
            else { return _blogpost; }



        }


        public List<Blogpost>? deleteById(int id)
        {

            Blogpost? _blogpost = _blogposts.FirstOrDefault(b => b.ID == id);

            if (_blogposts.Remove(_blogpost)) { return _blogposts; } else { return null; }

        }


        public Blogpost? createnewBlogpost(BlogPostRequestDTO blogpost)
        {


            Random random = new Random();
            int uniekgetal;

            do { uniekgetal = random.Next(0, 1001); }
            while (_blogposts.Any(bp => bp.ID == uniekgetal));


            Blogpost newBlogpost = new Blogpost(uniekgetal, blogpost.titel, blogpost.inhoud, blogpost.publicatieDatum);


            _blogposts.Add(newBlogpost);

            return newBlogpost;

        }

        public Blogpost? updateBlogpost(int id, BlogPostRequestDTO blogpost)
        {

            Blogpost? _blogpost = _blogposts.FirstOrDefault(bg => bg.ID == id);




            if (_blogpost == null) { return null; }
            else
            {

                _blogpost.titel = blogpost.titel;
                _blogpost.inhoud = blogpost.inhoud;
                _blogpost.publicatieDatum = blogpost.publicatieDatum;


                return _blogpost;
            }





        }





    }
}
