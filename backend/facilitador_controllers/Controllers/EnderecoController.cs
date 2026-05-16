using facilitador_domain.Domain.DTOs;
using facilitador_api.Application.Interfaces;
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

        [HttpGet]
        public async Task<IActionResult> ObterEnderecos()
        {
            var resultado = await _service.BuscarEnderecos();
            if (resultado == null || !resultado.Any())
            {
                return NotFound("Nenhum endereço encontrado.");
            }
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CriarEndereco(EnderecoCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);
            if (!resultado)
            {
                return BadRequest("Erro ao criar o endereço: " + resultado);
            }
            return Ok("Endereço criado com sucesso: " + resultado);
        }

        [HttpPatch]
        public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);
            if (!resultado)
            {
                return BadRequest("Erro ao atualizar o endereço: " + resultado);
            }
            return Ok("Endereço atualizado com sucesso: " + resultado);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarEndereco(Guid id)
        {
            var resultado = await _service.Desativar(id);
            if (!resultado)
            {
                return BadRequest("Erro ao deletar o endereço: " + resultado);
            }
            return Ok("Endereço deletado com sucesso: " + resultado);
        }
    }
}
