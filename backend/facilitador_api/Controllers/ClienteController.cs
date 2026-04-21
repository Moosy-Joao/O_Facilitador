using facilitador_api.Application.DTOs;
using facilitador_api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.API.Controllers
{
    [ApiController]
    [Route("api/v1/cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterClientes()
        {
            var resultado = await Task.FromResult(new List<ClienteDTO>());
            if (resultado == null || !resultado.Any())
            {
                return NotFound("Nenhum cliente encontrado.");
            }
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente(ClienteCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);

            if (resultado == true)
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
}