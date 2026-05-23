using facilitador_api.Application.Interfaces;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.API.Controllers
{
    [Route("api/v1/empresa")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public EmpresaController(IEmpresaService service)
        {
            _service = service;
        }

        [HttpGet("obter", Name = "ObterEmpresas")]
        [ProducesResponseType(typeof(EmpresaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterEmpresas()
        {
            var resultado = await _service.BuscarEmpresas();

            if (resultado == null || !resultado.Any())
            {
                return NotFound("Nenhuma empresa encontrada: " + resultado);
            }

            return Ok(resultado);
        }

        [HttpGet("obterporid/{id:guid}", Name = "ObterEmpresaPorId")]
        [ProducesResponseType(typeof(EmpresaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterEmpresaPorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);
            if (resultado == null)
            {
                return NotFound("Empresa não encontrada: " + resultado);
            }
            return Ok(resultado);
        }

        [HttpPost("criar", Name = "CriarEmpresa")]
        [ProducesResponseType(typeof(EmpresaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarEmpresa(EmpresaCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);

            if (!resultado)
            {
                return BadRequest("Erro ao criar a empresa: " + resultado);
            }

            return Ok("Empresa criada com sucesso: " + resultado);
        }

        [HttpPut("atualizar/{id:guid}", Name = "AtualizarEmpresa")]
        [ProducesResponseType(typeof(EmpresaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarEmpresa(Guid id, EmpresaUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);
            if (!resultado)
            {
                return BadRequest("Erro ao atualizar a empresa: " + resultado);
            }
            return Ok("Empresa atualizada com sucesso: " + resultado);
        }

        [HttpPut("ativar/{id:guid}", Name = "AtivarEmpresa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtivarEmpresa(Guid id)
        {
            var resultado = await _service.Ativar(id);
            if (!resultado)
            {
                return BadRequest("Erro ao ativar a empresa: " + resultado);
            }
            return Ok("Empresa ativada com sucesso: " + resultado);
        }

        [HttpDelete("desativar/{id:guid}", Name = "DesativarEmpresa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarEmpresa(Guid id)
        {
            var resultado = await _service.Desativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao desativar a empresa: " + resultado);
            }

            return Ok("Empresa desativada com sucesso: " + resultado);
        }
    }
}
