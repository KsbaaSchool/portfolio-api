





using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EersteWebNetApplicatie.DTOs;


namespace EersteWebNetApplicatie.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        readonly UserManager<IdentityUser> userManager;

        readonly SignInManager<IdentityUser> signinmangaer;

        public AccountController(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> Signinmanager)
        {

            this.userManager = UserManager;
            this.signinmangaer = Signinmanager;


        }


            [HttpPost("register")]
        public async Task<ActionResult> register(RegisterDTO dto)
        {

            IdentityUser user = new IdentityUser { UserName = dto.email, Email = dto.email };
            IdentityResult result = await userManager.CreateAsync(user, dto.wachtwoord);





            if (!result.Succeeded) { return BadRequest(result.Errors); }
            else
            {
                await userManager.AddToRoleAsync(user, "Gast");
                return Ok();
            }
        }





        [HttpPost("login")]
        public async Task<ActionResult> login(LoginDTO dto) {


            IdentityUser? user = await userManager.FindByEmailAsync(dto.email);
            if (user == null) { return Unauthorized(); }



            Boolean wwGoed = await userManager.CheckPasswordAsync(user,dto.wachtwoord);


            // Claude zei dat het zo sneller kan:
            // SigninResult result = await signinmangaer.PasswordSignInAsync(user, dto.wachtwoord, false, false);       (btw siginresult bestaat ook in mvc niet vergeten)

            if (wwGoed)
            {

                await signinmangaer.SignInAsync(user, false);

                return Ok(); 
            }

            return Unauthorized();

        
        }


    }
    
}
