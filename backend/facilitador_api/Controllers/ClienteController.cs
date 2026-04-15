using facilitador_api.Application.DTOs;
using facilitador_api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.API.Controllers
{
    [ApiController]
    [Route("Cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CriarCliente(ClienteDTO dto)
        {
            var resultado = _service.Criar(dto);

            if (resultado != "Cliente cadastrado com sucesso")
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
}