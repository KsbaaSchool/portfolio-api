using EersteWebNetApplicatie.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace EersteWebNetApplicatie.Tests
{
    public class BlogpostTests : IClassFixture<TestFactory> {


        readonly HttpClient client;
        readonly TestFactory factory;

        public BlogpostTests(TestFactory factory) {

            client = factory.CreateClient();

            this.factory = factory;

        }



        [Fact]
        public async Task GetAll_ZonderLogin_Geeft200EnLijst() {

            HttpClient editor = await factory.GetClient("Editor");


            HttpResponseMessage maakBlogpost = await editor.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Mijn Test Blogpost",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now

            });


            var dto = await maakBlogpost.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int id = dto.ID;



            HttpResponseMessage reactie = await client.GetAsync("/BlogPost");



            var dtolist = await reactie.Content.ReadFromJsonAsync<List<BlogPostResponseDTO>>();

            Assert.Equal(HttpStatusCode.OK, reactie.StatusCode);



            Assert.Contains(dtolist, b => b.ID == id);

        }



        [Fact]
        public async Task GetOpId_ZonderLogin_Geeft200() {

            HttpClient editor = await factory.GetClient("Editor");

            HttpClient geenloginClient = factory.CreateClient();

            HttpResponseMessage responseBlogpost = await editor.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Mijn Test Blogpost",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });


            var dto = await responseBlogpost.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int id = dto.ID;

            HttpResponseMessage reactie = await geenloginClient.GetAsync("/BlogPost/" + id);



            Assert.Equal(HttpStatusCode.OK, reactie.StatusCode);

        }

        [Fact]
        public async Task PostEnGet_AlsEditor_Geeft201En200() {


            HttpClient clientAsEditor = await factory.GetClient("Editor");

            HttpResponseMessage blogpostResponse = await clientAsEditor.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Mijn Test Blogpost",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await blogpostResponse.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int blogpostId = dto.ID;

            HttpResponseMessage blogpostById = await clientAsEditor.GetAsync("/BlogPost/" + blogpostId);

            Assert.Equal(HttpStatusCode.Created, blogpostResponse.StatusCode);

            Assert.Equal(HttpStatusCode.OK, blogpostById.StatusCode);


        }


        [Fact]
        public async Task GetOpId_OnbekendId_Geeft404()
        {

            HttpResponseMessage responseBlogpost = await client.GetAsync("/BlogPost/0");


            Assert.Equal(HttpStatusCode.NotFound, responseBlogpost.StatusCode);

        }

        [Fact]
        public async Task Post_ZonderLogin_Geeft401() {

            HttpResponseMessage postresponse = await client.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Mijn Test Blogpost Zonderlogin",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.Unauthorized, postresponse.StatusCode);


        }

        [Fact]
        public async Task Post_AlsGast_Geeft403()
        {

            HttpClient gast = await factory.GetClient("Gast");

            HttpResponseMessage post = await gast.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Mijn Test Blogpost",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.Forbidden, post.StatusCode);


        }

        [Fact]
        public async Task Post_AlsAdmin_Geeft201()
        {

            HttpClient admin = await factory.GetClient("Admin");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Mijn Test Blogpost",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.Created, post.StatusCode);


        }


        [Fact]
        public async Task Post_MetOngeldigeInvoer_Geeft400()
        {

            HttpClient admin = await factory.GetClient("Admin");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Mijn Test Blogpost",
                inhoud = "Dit is de inhoud van de testblogpost",
                // publicatieDatum weggelaten
            });


            Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);


        }

        [Fact]
        public async Task Put_ZonderLogin_Geeft401()
        {

            HttpClient admin = await factory.GetClient("Admin");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Blogpost aanmaken",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int dtoID = dto.ID;

            HttpResponseMessage put = await client.PutAsJsonAsync("/BlogPost/" + dtoID, new BlogPostRequestDTO
            {
                titel = "dan UPDATE NU (geupdate)",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.Unauthorized, put.StatusCode);


        }

        [Fact]
        public async Task Put_AlsGast_Geeft403()
        {

            HttpClient admin = await factory.GetClient("Admin");
            HttpClient gast = await factory.GetClient("Gast");


            HttpResponseMessage post = await admin.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Blogpost aanmaken",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int dtoID = dto.ID;

            HttpResponseMessage put = await gast.PutAsJsonAsync("/BlogPost/" + dtoID, new BlogPostRequestDTO
            {
                titel = "dan UPDATE NU (geupdate)",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.Forbidden, put.StatusCode);


        }




        [Fact]
        public async Task Put_AlsEditorEnGetLaatWijzigingZien_Geeft200()
        {

            HttpClient editor = await factory.GetClient("Editor");


            HttpResponseMessage post = await editor.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Blogpost aanmaken",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int dtoID = dto.ID;


            String nieuwetitel = "dan UPDATE NU (geupdate)";

            HttpResponseMessage put = await editor.PutAsJsonAsync("/BlogPost/" + dtoID, new BlogPostRequestDTO
            {
                titel = nieuwetitel,
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });


            HttpResponseMessage get = await editor.GetAsync("/BlogPost/" + dtoID);

            var getDto = await get.Content.ReadFromJsonAsync<BlogPostResponseDTO>();


            Assert.Equal(HttpStatusCode.OK, put.StatusCode);
            Assert.Equal(nieuwetitel, getDto.titel);

        }



        [Fact]
        public async Task Put_AlsAdmin_Geeft200()
        {

            HttpClient admin = await factory.GetClient("Admin");


            HttpResponseMessage post = await admin.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Blogpost aanmaken",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int dtoID = dto.ID;

            HttpResponseMessage put = await admin.PutAsJsonAsync("/BlogPost/" + dtoID, new BlogPostRequestDTO
            {
                titel = "dan UPDATE NU (geupdate) blogpost lesgoo",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.OK, put.StatusCode);


        }



        [Fact]
        public async Task Put_MetOngeldigeInvoer_Geeft400()
        {

            HttpClient admin = await factory.GetClient("Admin");


            HttpResponseMessage post = await admin.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Blogpost aanmaken",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int dtoID = dto.ID;

            HttpResponseMessage put = await admin.PutAsJsonAsync("/BlogPost/" + dtoID, new BlogPostRequestDTO
            {
                titel = "dan UPDATE NU (geupdate)",
                // inhoud weggelaten
                publicatieDatum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);


        }


        [Fact]
        public async Task Put_OpOnbekendId_Geeft404()
        {

            HttpClient admin = await factory.GetClient("Admin");


            HttpResponseMessage put = await admin.PutAsJsonAsync("/BlogPost/0", new BlogPostRequestDTO
            {
                titel = "dan UPDATE NU (geupdate)",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });


            Assert.Equal(HttpStatusCode.NotFound, put.StatusCode);


        }

        [Fact]
        public async Task Delete_ZonderLogin_Geeft401()
        {

            HttpClient admin = await factory.GetClient("Admin");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "delete mij",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int dtoID = dto.ID;



            HttpResponseMessage delete = await client.DeleteAsync("/BlogPost/" + dtoID);


            Assert.Equal(HttpStatusCode.Unauthorized, delete.StatusCode);


        }

        [Fact]
        public async Task Delete_AlsGast_Geeft403()
        {

            HttpClient admin = await factory.GetClient("Admin");
            HttpClient gast = await factory.GetClient("Gast");

            HttpResponseMessage post = await admin.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "delete mij",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int dtoID = dto.ID;



            HttpResponseMessage delete = await gast.DeleteAsync("/BlogPost/" + dtoID);


            Assert.Equal(HttpStatusCode.Forbidden, delete.StatusCode);


        }

        [Fact]
        public async Task DeleteEnGet_AlsEditor_Geeft200En404()
        {

            HttpClient editor = await factory.GetClient("Editor");


            HttpResponseMessage post = await editor.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Blogpost aanmaken",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int dtoID = dto.ID;



            HttpResponseMessage delete = await editor.DeleteAsync("/BlogPost/" + dtoID);


            HttpResponseMessage get = await editor.GetAsync("/BlogPost/" + dtoID);


            Assert.Equal(HttpStatusCode.OK, delete.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);

        }


        [Fact]
        public async Task Delete_AlsAdmin_Geeft200()
        {

            HttpClient admin = await factory.GetClient("Admin");


            HttpResponseMessage post = await admin.PostAsJsonAsync("/BlogPost", new BlogPostRequestDTO
            {
                titel = "Blogpost aanmaken",
                inhoud = "Dit is de inhoud van de testblogpost",
                publicatieDatum = DateTime.Now
            });

            var dto = await post.Content.ReadFromJsonAsync<BlogPostResponseDTO>();

            int dtoID = dto.ID;



            HttpResponseMessage delete = await admin.DeleteAsync("/BlogPost/" + dtoID);



            Assert.Equal(HttpStatusCode.OK, delete.StatusCode);

        }



        [Fact]
        public async Task Delete_MetOnbekendId_Geeft404()
        {

            HttpClient admin = await factory.GetClient("Admin");



            HttpResponseMessage delete = await admin.DeleteAsync("/BlogPost/0");



            Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);

        }

    }
}
