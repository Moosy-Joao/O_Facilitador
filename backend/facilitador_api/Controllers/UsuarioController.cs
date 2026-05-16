using facilitador_api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.Controllers
{
    [ApiController]
    [Route("api/v1/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _service.BuscarUsuarios();
            return Ok(usuarios);
        }
    }
}
