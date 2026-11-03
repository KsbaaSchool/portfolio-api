using EersteWebNetApplicatie.Controllers;
using EersteWebNetApplicatie.DTOs;
using EersteWebNetApplicatie.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;

namespace EersteWebNetApplicatie.Tests
{
    public class ProjectsTests : IClassFixture<TestFactory> {


        readonly HttpClient client;
        readonly TestFactory factory;

        public ProjectsTests(TestFactory factory) {


            //what about je maakt hier gwn alle rollen alvast aan dan heb je voor leke test ready kost schrijf werk 
            //(vraag aan docent)
            //of like watkan en mag in constructor alvast
            // hoe compacter makkelijk kunnen gedaan

            client = factory.CreateClient();

            this.factory = factory;

        }



        [Fact]
        public async Task GetAll_ZonderLogin_Geeft200EnLijst() {

            HttpClient editor = await factory.GetClient("Editor");


            HttpResponseMessage maakProject = await editor.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Mijn Test Project",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now

            });


            var dto = await maakProject.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int id = dto.ID;

            

            HttpResponseMessage reactie = await client.GetAsync("/Projects");



            var dtolist = await reactie.Content.ReadFromJsonAsync<List<ProjectResponseDTO>>();
            
            Assert.Equal(HttpStatusCode.OK, reactie.StatusCode);



            Assert.Contains(dtolist, p => p.ID == id);

        }



         [Fact]
        public async Task GetOpId_ZonderLogin_Geeft200() {

            HttpClient editor = await factory.GetClient("Editor");

            HttpClient geenloginClient = factory.CreateClient();

            HttpResponseMessage responseproject = await editor.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Mijn Test Project",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });


            var dto = await responseproject.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int id = dto.ID;

            HttpResponseMessage reactie = await geenloginClient.GetAsync("Projects/" + id);



            Assert.Equal(HttpStatusCode.OK, reactie.StatusCode);
        
        } 

        [Fact]

        public async Task PostEnGet_AlsEditor_Geeft201En200() {
            

            HttpClient clientAsEditor = await factory.GetClient("Editor");

            HttpResponseMessage projectResponse = await clientAsEditor.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Mijn Test Project",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await projectResponse.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int projectId = dto.ID;

            HttpResponseMessage projectById = await clientAsEditor.GetAsync("/Projects/" + projectId);

            Assert.Equal(HttpStatusCode.Created, projectResponse.StatusCode);

            Assert.Equal(HttpStatusCode.OK, projectById.StatusCode);


        }


        [Fact]
        public async Task GetOpId_OnbekendId_Geeft404()
        {



            HttpResponseMessage responseproject = await client.GetAsync("/Projects/0");
      






            Assert.Equal(HttpStatusCode.NotFound, responseproject.StatusCode);

        }

        [Fact]
        public async Task Post_ZonderLogin_Geeft401() {

            HttpResponseMessage postresponse = await client.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Mijn Test Project Zonderlogin",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.Unauthorized, postresponse.StatusCode);
        
        
        }

        [Fact]
        public async Task Post_AlsGast_Geeft403()
        {

            HttpClient gast = await factory.GetClient("Gast");

            HttpResponseMessage post = await gast.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Mijn Test Project",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.Forbidden, post.StatusCode);


        }

        [Fact]
        public async Task Post_AlsAdmin_Geeft201()
        {

            HttpClient admin = await factory.GetClient("Admin");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Mijn Test Project",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.Created, post.StatusCode);


        }


        [Fact]
        public async Task Post_MetOngeldigeInvoer_Geeft400()
        {

            HttpClient admin = await factory.GetClient("Admin");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Mijn Test Project",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
               // Date heb ik wel gehouden
            });


            Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);


        }

        [Fact]
        public async Task Put_ZonderLogin_Geeft401()
        {

            HttpClient admin = await factory.GetClient("Admin");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Post aanmaken",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int dtoID = dto.ID;

            HttpResponseMessage put = await client.PutAsJsonAsync("/Projects/" + dtoID, new ProjectRequestDTO
            {
                titel = "dan UPDATE NU (geupdate)",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now            });


            Assert.Equal(HttpStatusCode.Unauthorized, put.StatusCode);


        }

        [Fact]
        public async Task Put_AlsGast_Geeft403()
        {

            HttpClient admin = await factory.GetClient("Admin");
            HttpClient gast = await factory.GetClient("Gast");


            HttpResponseMessage post = await admin.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Post aanmaken",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int dtoID = dto.ID;

            HttpResponseMessage put = await gast.PutAsJsonAsync("/Projects/" + dtoID, new ProjectRequestDTO
            {
                titel = "dan UPDATE NU (geupdate)",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.Forbidden, put.StatusCode);


        }




        [Fact]
        public async Task Put_AlsEditorEnGetLaatWijzigingZien_Geeft200()
        {

            HttpClient editor = await factory.GetClient("Editor");


            HttpResponseMessage post = await editor.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Post aanmaken",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int dtoID = dto.ID;


            String nieuwetitel = "dan UPDATE NU (geupdate)";

            HttpResponseMessage put = await editor.PutAsJsonAsync("/Projects/" + dtoID, new ProjectRequestDTO
            {
                titel = nieuwetitel,
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });


            HttpResponseMessage get = await editor.GetAsync("/Projects/" + dtoID);

            var getDto = await get.Content.ReadFromJsonAsync<ProjectResponseDTO>();


            Assert.Equal(HttpStatusCode.OK, put.StatusCode);
            Assert.Equal(nieuwetitel, getDto.titel);

        }



        [Fact]
        public async Task Put_AlsAdmin_Geeft200()
        {

            HttpClient admin = await factory.GetClient("Admin");


            HttpResponseMessage post = await admin.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Post aanmaken",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int dtoID = dto.ID;

            HttpResponseMessage put = await admin.PutAsJsonAsync("/Projects/" + dtoID, new ProjectRequestDTO
            {
                titel = "dan UPDATE NU (geupdate)",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.OK, put.StatusCode);


        }



        [Fact]
        public async Task Put_MetOngeldigeInvoer_Geeft400()
        {

            HttpClient admin = await factory.GetClient("Admin");


            HttpResponseMessage post = await admin.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Post aanmaken",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int dtoID = dto.ID;

            HttpResponseMessage put = await admin.PutAsJsonAsync("/Projects/" + dtoID, new ProjectRequestDTO
            {
                titel = "dan UPDATE NU (geupdate)",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                //github link url weg gelaten
                Datum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);


        }


        [Fact]
        public async Task Put_OpVerkeerdeID_Geeft404()
        {

            HttpClient admin = await factory.GetClient("Admin");


   

     

            HttpResponseMessage put = await admin.PutAsJsonAsync("/Projects/0", new ProjectRequestDTO
            {
                titel = "dan UPDATE NU (geupdate)",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.NotFound, put.StatusCode);


        }

        [Fact]
        public async Task Delete_ZonderLogin_Geeft401()
        {

            HttpClient admin = await factory.GetClient("Admin");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "delete mij",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int dtoID = dto.ID;



            HttpResponseMessage delete = await client.DeleteAsync("/Projects/" + dtoID);


            Assert.Equal(HttpStatusCode.Unauthorized, delete.StatusCode);


        }

        [Fact]
        public async Task Delete_AlsGast_Geeft403()
        {

            HttpClient admin = await factory.GetClient("Admin");
            HttpClient gast = await factory.GetClient("Gast");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "delete mij",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int dtoID = dto.ID;



            HttpResponseMessage delete = await gast.DeleteAsync("/Projects/" + dtoID);


            Assert.Equal(HttpStatusCode.Forbidden, delete.StatusCode);


        }

        [Fact]
        public async Task DeleteEnGet_AlsEditor_Geeft200En404()
        {

            HttpClient editor = await factory.GetClient("Editor");


            HttpResponseMessage post = await editor.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Post aanmaken",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int dtoID = dto.ID;



            HttpResponseMessage delete = await editor.DeleteAsync("/Projects/" + dtoID);


            HttpResponseMessage get = await editor.GetAsync("/Projects/" + dtoID);

            var getDto = await get.Content.ReadFromJsonAsync<ProjectResponseDTO>();


            Assert.Equal(HttpStatusCode.OK, delete.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);

        }


        [Fact]
        public async Task Delete_AlsAdminr_Geeft200()
        {

            HttpClient admin = await factory.GetClient("Admin");


            HttpResponseMessage post = await admin.PostAsJsonAsync("/Projects", new ProjectRequestDTO
            {
                titel = "Post aanmaken",
                beschrijving = "Dit is een testbeschrijving voor het project",
                Categorie = "Webdevelopment",
                GitHubUrl = "https://github.com/gebruiker/repo",
                Datum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<ProjectResponseDTO>();

            int dtoID = dto.ID;



            HttpResponseMessage delete = await admin.DeleteAsync("/Projects/" + dtoID);





            Assert.Equal(HttpStatusCode.OK, delete.StatusCode);

        }



        [Fact]
        public async Task Delete_MetOnbekendID_404()
        {

            HttpClient admin = await factory.GetClient("Admin");




            HttpResponseMessage delete = await admin.DeleteAsync("/Projects/0");





            Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);

        }

    }
}
