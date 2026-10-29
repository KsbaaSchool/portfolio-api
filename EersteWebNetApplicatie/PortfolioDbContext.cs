using EersteWebNetApplicatie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace EersteWebNetApplicatie
{
    public class PortfolioDbContext : IdentityDbContext<IdentityUser>
    {

        public PortfolioDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { } 


        public DbSet<Blogpost> Blogposts { get; set;  }
            public DbSet<Project> Projects { get; set; }



    }
}
