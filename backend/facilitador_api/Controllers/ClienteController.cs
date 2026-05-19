using facilitador_api.Application.Interfaces;
using facilitador_domain.Domain.DTOs;
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

        [HttpGet(Name = "ObterClientes")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterClientes()
        {
            var result = await _service.BuscarClientes();
            return Ok(result);
        }

        [HttpPost(Name = "CriarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarCliente(ClienteCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);

            if (resultado == false)
            {
                return BadRequest("Erro ao criar cliente." + true);
            }

            return Ok(resultado);
        }

        [HttpPatch("{id:guid}", Name = "AtualizarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarCliente(Guid id, ClienteUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);
            if (resultado == false)
            {
                return BadRequest("Erro ao atualizar cliente." + true);
            }
            return Ok(resultado);
        }

        [HttpPost("{id:guid}", Name = "AtivarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtivarCliente(Guid id)
        {
            var resultado = await _service.Ativar(id);
            if (resultado == false)
            {
                return BadRequest("Erro ao ativar cliente.");
            }
            return Ok(resultado);
        }

        [HttpDelete("{id:guid}", Name = "DesativarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
