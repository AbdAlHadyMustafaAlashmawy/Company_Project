using Company_Project.DTO;
using Company_Project.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XAct;

namespace Company_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController:ControllerBase
    {
        private readonly UserManager<ApplicationUser> _usermaneger;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            _usermaneger = userManager;
            _configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Registeration(RegisterUserDTO registerUserDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser User = new ApplicationUser();
                User.UserName=registerUserDTO.UserName;
                User.Email=registerUserDTO.Email;
                var result = await _usermaneger.CreateAsync(User, registerUserDTO.Password);
                if (result.Succeeded)
                {
                    return Ok("Record created successfully !!");
                }
                else
                {
                    string ErrorMessage = "";
                    foreach (var item in result.Errors)
                    {
                        ErrorMessage += item.Description + "\n";
                    }
                    return BadRequest(ErrorMessage);
                }
            }
            return BadRequest(ModelState);

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginAccDTO AccDto)
        {
            if(ModelState.IsValid)
            {
              var userApp= await _usermaneger.FindByNameAsync(AccDto.UserName);
                if (userApp!=null)
                {
                    bool found  = await _usermaneger.CheckPasswordAsync(userApp, AccDto.Password);
                    if(found)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, AccDto.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, userApp.Id ));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await _usermaneger.GetRolesAsync(userApp);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                        SigningCredentials signing= new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                            (
                            issuer: _configuration["JWT:issuerValid"],
                            audience: _configuration["JWT:audienceValid"]
                           ,claims: claims,
                            expires:DateTime.Now.AddHours(1),
                            signingCredentials: signing
                            );

                        return Ok
                            (
                               new
                               {
                                   token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                                   expires=jwtSecurityToken.ValidTo
                               }
                            );
                    }

                }
                return Unauthorized();
            }
            return BadRequest(ModelState);
        }
       
    }
}
