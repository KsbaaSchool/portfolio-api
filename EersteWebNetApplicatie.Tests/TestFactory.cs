using EersteWebNetApplicatie.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace EersteWebNetApplicatie.Tests
{
    public class TestFactory : WebApplicationFactory<Program>
    {


        protected override void ConfigureWebHost(IWebHostBuilder builder) {

            builder.UseEnvironment("Testing");
        
        

         }



        public async Task<HttpClient> GetClient(String rolnaam) {


            String email = Guid.NewGuid() + "@test.nl";
            String wachtwoord = "Asd1." + Guid.NewGuid();

            HttpClient client = CreateClient();

            HttpResponseMessage registratie = await client.PostAsJsonAsync("/Account/register", new RegisterDTO { email = email, wachtwoord = wachtwoord });

            registratie.EnsureSuccessStatusCode();

            if (rolnaam != "Gast") {

                using (var scope = Services.CreateScope())
                {

                    UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();


                    


                    IdentityUser user = await userManager.FindByEmailAsync(email);

                    await userManager.AddToRoleAsync(user, rolnaam);




                }







            }



            HttpResponseMessage login = await client.PostAsJsonAsync("/Account/login", new LoginDTO { email = email, wachtwoord = wachtwoord });

            login.EnsureSuccessStatusCode();


            return client;


        }




    }
}
