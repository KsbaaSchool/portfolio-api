using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EersteWebNetApplicatie
{
    public class ProjectService
    {
        /*  private static List<Project> _projects = new List<Project> {

              new Project("Project 1", 324, "Beschrijving 1", "Categorie 1", "https://github.com/project1", new DateTime(2026, 10, 1)),

                  new Project("Project 2", 112, "Beschrijving 1", "Categorie 1", "https://github.com/project1", new DateTime(2026, 10, 1))

              }; */


        readonly PortfolioDbContext context;
        public ProjectService(PortfolioDbContext context) { this.context = context; }



        public async Task<List<Project>> getProjecten()
        {



                return await context.Projects.ToListAsync();


        }




            public async Task<Project?> getProjectByID(int id)
            {


                

            return await context.Projects.FirstOrDefaultAsync(p => p.ID == id); 

        }

        public async Task<List<Project>?> deleteProjectByID(int id)
        {

            Project? _project = await context.Projects.FirstOrDefaultAsync(project => id == project.ID);

            if (_project == null) { return null; }
            else
            {
                context.Projects.Remove(_project);

                await context.SaveChangesAsync();

                return await context.Projects.ToListAsync();


            }
        }





        public async Task<Project> createnewProject(ProjectRequestDTO project)

        {


            Project _project = new Project(project.titel, 0, project.beschrijving,
                                            project.Categorie, project.GitHubUrl, project.Datum);

            context.Projects.Add(_project);
           await context.SaveChangesAsync();


      




            return _project;


        }




        public async Task<Project?> updateProjectbyID(int id, ProjectRequestDTO project)
        {

            Project? _project = await context.Projects.FirstOrDefaultAsync(p => p.ID == id);

            if (_project == null) { return null; }
            else
            {
                _project.titel = project.titel;
                _project.beschrijving = project.beschrijving;
                _project.Categorie = project.Categorie;
                _project.GitHubUrl = project.GitHubUrl;
                _project.Datum = project.Datum;

                await context.SaveChangesAsync();
                return _project;



            }

        }









    }
}
