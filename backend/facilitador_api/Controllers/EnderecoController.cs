using facilitador_api.Application.DTOs;
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
            var resultado = await Task.FromResult(new List<EnderecoDTO>());
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
    }
}
