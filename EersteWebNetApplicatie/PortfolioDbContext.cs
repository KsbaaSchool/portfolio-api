using EersteWebNetApplicatie.Models;
using Microsoft.EntityFrameworkCore;

namespace EersteWebNetApplicatie
{
    public class PortfolioDbContext : DbContext
    {

        public PortfolioDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { } 


        public DbSet<Blogpost> Blogposts { get; set;  }
            public DbSet<Project> Projects { get; set; }

                






    }
}
