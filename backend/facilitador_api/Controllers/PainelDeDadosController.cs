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

        //[HttpGet("stats")]
        //public async Task<IActionResult> GetStats()
        //{
        //    try
        //    {
        //        var totalClients = await _context.Clientes.CountAsync(c => c.Ativo);
        //        var comprasQuery = _context.Compras.Where(c => c.Ativo);
        //        var pagamentosQuery = _context.Pagamentos.Where(p => p.Ativo);

        //        var totalReceber = await comprasQuery.SumAsync(c => c.Valor) - await pagamentosQuery.SumAsync(p => p.ValorPagamento);

        //        var today = DateTime.UtcNow.Date;
        //        var vendasHoje = await comprasQuery.Where(c => c.CriadoEm >= today).SumAsync(c => c.Valor);
        //        var pagamentosHoje = await pagamentosQuery.Where(p => p.DataPagamento >= today).SumAsync(p => p.ValorPagamento);

        //        var clientsList = await _context.Clientes.Where(c => c.Ativo).ToListAsync();
        //        var inadimplentes = clientsList.Count(c => c.Saldo > c.LimiteCredito);
        //        var inadimplentesValor = clientsList.Where(c => c.Saldo > c.LimiteCredito).Sum(c => c.Saldo - c.LimiteCredito);

        //        var startOfWeek = today.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
        //        var novosClientesSemana = await _context.Clientes.CountAsync(c => c.CriadoEm >= startOfWeek);

        //        return Ok(new
        //        {
        //            totalReceber = totalReceber,
        //            totalReceberVar = 0,
        //            inadimplentes = inadimplentes,
        //            inadimplentesValor = inadimplentesValor,
        //            totalClientes = totalClients,
        //            novosClientesSemana = novosClientesSemana,
        //            vendasHoje = vendasHoje,
        //            pagamentosHoje = pagamentosHoje
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Erro ao carregar dados do dashboard: " + ex.Message);
        //    }
        //}

        //[HttpGet("transactions")]
        //public async Task<IActionResult> GetTransactions()
        //{
        //    try
        //    {
        //        var compras = await _context.Compras
        //            .Include(c => c.Cliente)
        //            .OrderByDescending(c => c.CriadoEm)
        //            .Take(5)
        //            .Select(c => new { id = c.Id, type = "venda", cliente = c.Cliente.Nome, valor = c.Valor, hora = c.CriadoEm.ToString("HH:mm"), data = c.CriadoEm, status = c.Ativo ? "concluido" : "estornado" })
        //            .ToListAsync();

        //        var pagamentos = await _context.Pagamentos
        //            .Include(p => p.Cliente)
        //            .OrderByDescending(p => p.DataPagamento)
        //            .Take(5)
        //            .Select(p => new { id = p.Id, type = "pagamento", cliente = p.Cliente.Nome, valor = p.ValorPagamento, hora = p.DataPagamento.ToString("HH:mm"), data = p.DataPagamento, status = p.Ativo ? "concluido" : "estornado" })
        //            .ToListAsync();

        //        var transactions = compras.Cast<dynamic>()
        //            .Concat(pagamentos.Cast<dynamic>())
        //            .OrderByDescending(t => t.data)
        //            .Take(5)
        //            .ToList();

        //        return Ok(transactions);
        //    }
        //    catch (Exception)
        //    {
        //        return Ok(new object[] { });
        //    }
        //}

        //[HttpGet("chart")]
        //public async Task<IActionResult> GetChartData()
        //{
        //    try
        //    {
        //        var thirtyDaysAgo = DateTime.UtcNow.Date.AddDays(-30);
        //        var compras = await _context.Compras
        //            .Where(c => c.Ativo && c.CriadoEm >= thirtyDaysAgo)
        //            .GroupBy(c => c.CriadoEm.Date)
        //            .Select(g => new { Data = g.Key, Valor = g.Sum(c => c.Valor) })
        //            .ToListAsync();

        //        var result = new List<object>();
        //        for (int i = 29; i >= 0; i--)
        //        {
        //            var date = DateTime.UtcNow.Date.AddDays(-i);
        //            var match = compras.FirstOrDefault(c => c.Data == date);
        //            result.Add(new
        //            {
        //                date = date.ToString("dd/MM"),
        //                value = match != null ? match.Valor : 0
        //            });
        //        }

        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        return Ok(new object[] { });
        //    }
        //}
    }
}
