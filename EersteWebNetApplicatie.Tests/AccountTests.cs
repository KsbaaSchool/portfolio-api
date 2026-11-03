using EersteWebNetApplicatie.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace EersteWebNetApplicatie.Tests
{
    public class AccountTests : IClassFixture<TestFactory>
    {
        readonly HttpClient client;

       readonly TestFactory factory;

        public AccountTests(TestFactory factory)
        {

            client = factory.CreateClient();
            this.factory = factory;
        }


         [Fact]
        public async Task Registreer_MetGeldigeGegevens_Geeft200() {


            String email = Guid.NewGuid() + "@test.nl";
            String wachtwoord = "As1." + Guid.NewGuid;

            HttpResponseMessage registerResponse = await client.PostAsJsonAsync("/Account/register", new RegisterDTO {email = email , wachtwoord = wachtwoord });


            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);
        
        }


        [Fact]
        public async Task Registreer_MetOngeldigeEmail_Geeft400()
        {


            String email = "Faaaa";
            String wachtwoord = "As1." + Guid.NewGuid;

            HttpResponseMessage registerResponse = await client.PostAsJsonAsync("/Account/register", new RegisterDTO { email = email, wachtwoord = wachtwoord });


            Assert.Equal(HttpStatusCode.BadRequest, registerResponse.StatusCode);

        }

        [Fact]
        public async Task Registreer_MetBestaandeEmail_Geeft400()
        {


            String email = Guid.NewGuid() + "@test.nl";
            String wachtwoord = "As1." + Guid.NewGuid;

            HttpResponseMessage maakaccountmetemail = await client.PostAsJsonAsync("/Account/register", new RegisterDTO { email = email, wachtwoord = wachtwoord });


            HttpResponseMessage maakaccountNogEenKeermetemail = await client.PostAsJsonAsync("/Account/register", new RegisterDTO { email = email, wachtwoord = wachtwoord });

            Assert.Equal(HttpStatusCode.BadRequest, maakaccountNogEenKeermetemail.StatusCode);

        }



        [Fact]
        public async Task Post_AlsGast_Geeft403() {

            String email = Guid.NewGuid() + "@test.nl";
            String wachtwoord = "As1." + Guid.NewGuid;

            HttpResponseMessage newaccount = await client.PostAsJsonAsync("/Account/register", new RegisterDTO { email = email, wachtwoord = wachtwoord });

            HttpResponseMessage login = await client.PostAsJsonAsync("/Account/login", new LoginDTO { email = email, wachtwoord = wachtwoord });

            HttpResponseMessage post = await client.PostAsJsonAsync("/Projects", new ProjectRequestDTO
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
        public async Task Login_Geeft200EnCookie() {

            String email = Guid.NewGuid() + "@test.nl";
            String wachtwoord = "As1." + Guid.NewGuid;

            HttpResponseMessage register = await client.PostAsJsonAsync("/Account/register", new RegisterDTO { email = email, wachtwoord = wachtwoord });

            HttpResponseMessage login = await client.PostAsJsonAsync("/Account/login", new LoginDTO { email = email, wachtwoord = wachtwoord });


            Assert.Equal(HttpStatusCode.OK, login.StatusCode);

            Assert.True(login.Headers.Contains("Set-Cookie"));

        }



        [Fact]
        public async Task Login_MetFoutWW_Geeft401()
        {

            String email = Guid.NewGuid() + "@test.nl";
            String wachtwoord = "As1." + Guid.NewGuid;

            HttpResponseMessage register = await client.PostAsJsonAsync("/Account/register", new RegisterDTO { email = email, wachtwoord = wachtwoord });

            HttpResponseMessage loginmetFoutWW = await client.PostAsJsonAsync("/Account/login", new LoginDTO { email = email, wachtwoord = "fakewachtwoord" });


            Assert.Equal(HttpStatusCode.Unauthorized, loginmetFoutWW.StatusCode);



        }

        [Fact]
        public async Task Login_MetOnbekendeEmail_Geeft401()
        {

            String email = Guid.NewGuid() + "@test.nl";
            String wachtwoord = "As1." + Guid.NewGuid;

            HttpResponseMessage register = await client.PostAsJsonAsync("/Account/register", new RegisterDTO { email = email, wachtwoord = wachtwoord });

            HttpResponseMessage loginmetOnbekendEmail = await client.PostAsJsonAsync("/Account/login", new LoginDTO { email = Guid.NewGuid() + "@test.nl", wachtwoord = wachtwoord });


            Assert.Equal(HttpStatusCode.Unauthorized, loginmetOnbekendEmail.StatusCode);



        }

    }
}
