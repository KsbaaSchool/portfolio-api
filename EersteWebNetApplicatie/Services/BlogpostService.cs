using EersteWebNetApplicatie.DTO_s;
using EersteWebNetApplicatie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace EersteWebNetApplicatie.Services
{
    public class BlogpostService
    {

        readonly PortfolioDbContext context;

       public BlogpostService(PortfolioDbContext context)
        {

            this.context = context;

        }



        public async Task<List<BlogPostResponseDTO>> getBlogposts()
        {

            
            return await context.Blogposts.Select(bg => new BlogPostResponseDTO(bg.ID,bg.titel, bg.inhoud, bg.publicatieDatum)).ToListAsync();

        }


        public async Task<BlogPostResponseDTO?> getBlogpostByID(int id)
        {

            Blogpost? _blogpost = await context.Blogposts.FirstOrDefaultAsync(blogpost => id == blogpost.ID);

            if (_blogpost == null) { return null; }
            else
            {

                BlogPostResponseDTO responseBlogpost = new BlogPostResponseDTO(_blogpost.ID, _blogpost.titel, _blogpost.inhoud, _blogpost.publicatieDatum);


                return responseBlogpost;

            }

        }


        public async Task<List<BlogPostResponseDTO>?> deleteById(int id)
        {

            DbSet<Blogpost> _blogposts = context.Blogposts;
            Blogpost? _blogpost = await context.Blogposts.FirstOrDefaultAsync(b => b.ID == id);

            if (_blogpost == null)
            {
                return null; } else {

                _blogposts.Remove(_blogpost);

                await context.SaveChangesAsync();



                return await context.Blogposts.Select(bg => new BlogPostResponseDTO(bg.ID, bg.titel, bg.inhoud, bg.publicatieDatum)).ToListAsync();
            }

        }


        public async Task<BlogPostResponseDTO> createnewBlogpost(BlogPostRequestDTO blogpost)
        {


            DbSet<Blogpost> _blogposts = context.Blogposts;



            Blogpost newBlogpost = new Blogpost(0, blogpost.titel, blogpost.inhoud, blogpost.publicatieDatum);


            _blogposts.Add(newBlogpost);

            await context.SaveChangesAsync();


            BlogPostResponseDTO responseBlogpost = new BlogPostResponseDTO(newBlogpost.ID, newBlogpost.titel, newBlogpost.inhoud, newBlogpost.publicatieDatum);


            return responseBlogpost;

        }

        public async Task<BlogPostResponseDTO?> updateBlogpost(int id, BlogPostRequestDTO blogpost)
        {

            Blogpost? _blogpost = await context.Blogposts.FirstOrDefaultAsync(bg => bg.ID == id);




            if (_blogpost == null) { return null; }
            else
            {

                _blogpost.titel = blogpost.titel;
                _blogpost.inhoud = blogpost.inhoud;
                _blogpost.publicatieDatum = blogpost.publicatieDatum;

                await context.SaveChangesAsync();

                BlogPostResponseDTO responseBlogpost = new BlogPostResponseDTO(_blogpost.ID, _blogpost.titel, _blogpost.inhoud, _blogpost.publicatieDatum);


                return responseBlogpost;
            }





        }





    }
}
