using facilitador_api.Application.DTOs;
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
            var resultado = await Task.FromResult(new List<EmpresaDTO>());
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
    }
}
