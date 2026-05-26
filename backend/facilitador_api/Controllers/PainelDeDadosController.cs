using facilitador_application.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using facilitador_api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace facilitador_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/paineldados")]
    public class PainelDeDadosController : ControllerBase
    {
        private readonly IPainelDeDadosService _service;

        public PainelDeDadosController(IPainelDeDadosService service)
        {
            _service = service;
        }

        [HttpGet("dados", Name = "ObterDados")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var empresaId = User.ObterEmpresaId();
                var dados = await _service.ObterDados(empresaId);
                return Ok(dados);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao carregar dados do dashboard: {ex.Message}");
            }
        }

        [HttpGet("transacoes", Name = "ObterTransacoes")]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var empresaId = User.ObterEmpresaId();
                var transacoes = await _service.ObterTransacoesRecentes(empresaId);
                return Ok(transacoes);
            }
            catch (Exception)
            {
                return Ok(new List<object>());
            }
        }

        [HttpGet("grafico", Name = "ObterDadosGrafico")]
        public async Task<IActionResult> GetChartData()
        {
            try
            {
                var empresaId = User.ObterEmpresaId();
                var grafico = await _service.ObterDadosGraficoVendas(empresaId);
                return Ok(grafico);
            }
            catch (Exception)
            {
                return Ok(new List<object>());
            }
        }
    }
}
