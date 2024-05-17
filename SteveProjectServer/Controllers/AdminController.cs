using GameModel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteveProjectServer.DTO;
using System.IdentityModel.Tokens.Jwt;

namespace SteveProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<PublisherUser> userManager,
        JwtHandler jwtHandler) : ControllerBase
    {
        //We need method
        //NEVER use entity framework direct frame access
        //Only use microsoft identity methods to handle identity
        // We use post because we do not want to see the password in the URL.
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            // Question mark means it may be not; if the user doesn't exist, it returns null.
            string badlogin = "Bad username or password.";
            PublisherUser? user = await userManager.FindByNameAsync(loginRequest.username);
            if (user == null)
            {
                // return a 401 error
                return Unauthorized(badlogin);
            }
            bool success = await userManager.CheckPasswordAsync(user, loginRequest.password);
            if (!success)
            {
                return Unauthorized(badlogin);
            }
            // generate token for the user
            JwtSecurityToken token = await jwtHandler.GetTokenAsync(user);
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResult()
            {
                success = true,
                message = "Welcome",
                token = jwtToken
            });
        }
    }
}