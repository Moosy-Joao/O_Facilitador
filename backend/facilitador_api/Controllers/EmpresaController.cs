using facilitador_domain.Domain.DTOs;
using facilitador_api.Application.Interfaces;
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

        [HttpGet]
        public async Task<IActionResult> ObterEmpresas()
        {
            var resultado = await _service.BuscarEmpresas();
            if (resultado == null || !resultado.Any())
            {
                return NotFound("Nenhuma empresa encontrada.");
            }
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CriarEmpresa(EmpresaCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);

            if (!resultado)
            {
                return BadRequest("Erro ao criar a empresa: " + resultado);
            }

            return Ok("Empresa criada com sucesso: " + resultado);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarEmpresa(Guid id, EmpresaUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);
            if (!resultado)
            {
                return BadRequest("Erro ao atualizar a empresa: " + resultado);
            }
            return Ok("Empresa atualizada com sucesso: " + resultado);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarEmpresa(Guid id)
        {
            var resultado = await _service.Desativar(id);
            if (!resultado)
            {
                return BadRequest("Erro ao deletar a empresa: " + resultado);
            }
            return Ok("Empresa deletada com sucesso: " + resultado);
        }
    }
}
