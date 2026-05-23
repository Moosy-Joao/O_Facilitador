using facilitador_api.Application.Interfaces;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.Controllers
{
    [Route("api/v1/endereco")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly IEnderecoService _service;

        public EnderecoController(IEnderecoService service)
        {
            _service = service;
        }

        [HttpGet("obter", Name = "ObterEnderecos")]
        [ProducesResponseType(typeof(List<EnderecoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterEnderecos()
        {
            var resultado = await _service.BuscarEnderecos();
            if (resultado == null || !resultado.Any())
            {
                return NotFound("Nenhum endereço encontrado.");
            }
            return Ok(resultado);
        }

        [HttpGet("obterporid/{id:guid}", Name = "ObterEnderecoPorId")]
        [ProducesResponseType(typeof(EnderecoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterEnderecoPorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);
            if (resultado == null)
            {
                return NotFound("Endereço não encontrado: " + resultado);
            }
            return Ok(resultado);
        }

        [HttpPost("criar", Name = "CriarEndereco")]
        [ProducesResponseType(typeof(EnderecoResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarEndereco(EnderecoCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);
            if (!resultado)
            {
                return BadRequest("Erro ao criar o endereço: " + resultado);
            }
            return Ok("Endereço criado com sucesso: " + resultado);
        }

        [HttpPatch("atualizar/{id:guid}", Name = "AtualizarEndereco")]
        [ProducesResponseType(typeof(EnderecoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);
            if (!resultado)
            {
                return BadRequest("Erro ao atualizar o endereço: " + resultado);
            }
            return Ok("Endereço atualizado com sucesso: " + resultado);
        }

        [HttpDelete("desativar/{id:guid}", Name = "DesativarEndereco")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarEndereco(Guid id)
        {
            var resultado = await _service.Desativar(id);
            if (!resultado)
            {
                return BadRequest("Erro ao desativar o endereço: " + resultado);
            }
            return Ok("Endereço desativado com sucesso: " + resultado);
        }
    }
}
