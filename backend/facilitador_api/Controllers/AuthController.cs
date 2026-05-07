using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simple mock authentication for now, until full auth logic is required
            if (request.Username == "admin" && request.Password == "123456")
            {
                return Ok(new
                {
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.mockToken_backend_real",
                    User = new { Id = 1, Name = "Admin System", Role = "admin" }
                });
            }

            return Unauthorized(new { Message = "Usuário ou senha incorretos" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
