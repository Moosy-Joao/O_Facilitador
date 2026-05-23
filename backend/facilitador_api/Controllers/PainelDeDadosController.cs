using facilitador_application.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.Controllers
{
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
                var dados = await _service.ObterDados();
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
                var transacoes = await _service.ObterTransacoesRecentes();
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
                var grafico = await _service.ObterDadosGraficoVendas();
                return Ok(grafico);
            }
            catch (Exception)
            {
                return Ok(new List<object>());
            }
        }
    }
}
