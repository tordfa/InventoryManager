using BlazorApp1.Server.Authentication;
using BlazorApp1.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("api/login/")]
        [AllowAnonymous]
        public ActionResult<UserSessionModel> Login([FromBody] LoginRequest loginRequest)
        {
            var jwtAuthenticationManager = new JwtAuthenticationManager();
            var userSession = jwtAuthenticationManager.GenerateJwtToken(loginRequest.Username, loginRequest.Password);
            if (userSession is null)
            {
                return Unauthorized();
            }
            else
            {
                return userSession;
            }

        }
    }
}
