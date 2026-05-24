using facilitador_api.Application.Interfaces;
using facilitador_api.Helpers;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obter", Name = "ObterClientes")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterClientes()
        {
            var resultado = await _service.BuscarClientes();

            return Ok(resultado ?? new List<ClienteResponseDTO>());
        }

        [HttpGet("obterporid/{id:guid}", Name = "ObterClientePorId")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterClientePorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);
            if (resultado == null)
            {
                return NotFound("Cliente não encontrado: " + resultado);
            }
            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("inadimplentes", Name = "ObterInadimplentes")]
        [ProducesResponseType(typeof(List<ClienteInadimplenteResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObterInadimplentes([FromQuery] Guid empresaId, [FromQuery] int diasAtraso = 30)
        {
            // Validação simples
            if (empresaId == Guid.Empty)
                return BadRequest("O parâmetro 'empresaId' é obrigatório.");

            if (diasAtraso < 1)
                return BadRequest("O parâmetro 'diasAtraso' deve ser maior que zero.");

            // Verifica se o usuário autenticado pertence à empresa solicitada
            var empresaDoToken = User.ObterEmpresaId();
            if (empresaDoToken != empresaId)
                return Forbid("Você não tem permissão para acessar dados de outra empresa.");

            var resultado = await _service.ObterInadimplentes(empresaId, diasAtraso);
            return Ok(resultado);
        }

        [HttpPost("criar", Name = "CriarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarCliente(ClienteCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);

            if (resultado == false)
            {
                return BadRequest("Erro ao criar cliente: " + resultado);
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpPatch("atualizar/{id:guid}", Name = "AtualizarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarCliente(Guid id, ClienteUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);

            if (resultado == false)
            {
                return BadRequest("Erro ao atualizar cliente: " + resultado);
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpPost("ativar/{id:guid}", Name = "AtivarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtivarCliente(Guid id)
        {
            var resultado = await _service.Ativar(id);

            if (resultado == false)
            {
                return BadRequest("Erro ao ativar cliente: " + resultado);
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpDelete("desativar/{id:guid}", Name = "DesativarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarCliente(Guid id)
        {
            var resultado = await _service.Desativar(id);

            if (resultado == false)
            {
                return BadRequest("Erro ao desativar cliente: " + resultado);
            }

            return Ok(resultado);
        }
    }
}
