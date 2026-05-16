using facilitador_domain.Domain.DTOs;
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
            var result = await _service.BuscarClientes();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente(ClienteCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);

            if (resultado == false)
            {
                return BadRequest("Erro ao criar cliente." + true);
            }

            return Ok(resultado);
        }

        [HttpPatch]
        public async Task<IActionResult> AtualizarCliente(Guid id, ClienteUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);
            if (resultado == false)
            {
                return BadRequest("Erro ao atualizar cliente." + true);
            }
            return Ok(resultado);
        }

        [HttpPost("ativar")]
        public async Task<IActionResult> AtivarCliente(Guid id)
        {
            var resultado = await _service.Ativar(id);
            if (resultado == false)
            {
                return BadRequest("Erro ao ativar cliente.");
            }
            return Ok(resultado);
        }

        [HttpDelete("desativar")]
        public async Task<IActionResult> DesativarCliente(Guid id)
        {
            var resultado = await _service.Desativar(id);
            if (resultado == false)
            {
                return BadRequest("Erro ao desativar cliente.");
            }
            return Ok(resultado);
        }
    }
}
