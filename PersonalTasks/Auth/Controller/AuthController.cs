using Microsoft.AspNetCore.Mvc;
using PersonalTasks.Auth.Controller.DTOs.Request;
using PersonalTasks.Auth.Services;
using PersonalTasks.Models;


namespace PersonalTasks.Auth.Controller
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserRequest user)
        {
            await _authService.RegisterUser(user);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest user)
        {
            User? userWithPassword = await _authService.GetUserWithPasswordByUsername(user.Username);

            if (userWithPassword == null)
            {
                return NotFound();
            }

            if (!(BCrypt.Net.BCrypt.Verify(user.Password, userWithPassword.PasswordHashed))) // Compare the hashed password with the password provided by the user
            {
                return Unauthorized();
            }

            string token = await _authService.GenerateJWT(userWithPassword);
            return Ok(token);
        }
    }
}
