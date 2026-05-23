using facilitador_api.Infrastructure.DB;
using facilitador_application.Application.Interfaces;
using facilitador_domain.Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace facilitador_application.Application.Services
{
    public class PainelDeDadosService : IPainelDeDadosService
    {
        private readonly ConnectionContext _context;

        public PainelDeDadosService(ConnectionContext context)
        {
            _context = context;
        }

        public async Task<PainelDeDadosDTO> ObterDados()
        {
            var totalClientes = await _context.Clientes.CountAsync(c => c.Ativo);
            var comprasQuery = _context.Compras.Where(c => c.Ativo);
            var pagamentosQuery = _context.Pagamentos.Where(p => p.Ativo);

            var totalReceber = await comprasQuery.SumAsync(c => c.Valor) -
                               await pagamentosQuery.SumAsync(p => p.ValorPagamento);

            var today = DateTime.UtcNow.Date;
            var vendasHoje = await comprasQuery.Where(c => c.CriadoEm >= today).SumAsync(c => c.Valor);
            var pagamentosHoje = await pagamentosQuery.Where(p => p.DataPagamento >= today).SumAsync(p => p.ValorPagamento);

            var clientesList = await _context.Clientes.Where(c => c.Ativo).ToListAsync();
            var inadimplentes = clientesList.Count(c => c.Saldo > c.LimiteCredito);
            var inadimplentesValor = clientesList
                .Where(c => c.Saldo > c.LimiteCredito)
                .Sum(c => c.Saldo - c.LimiteCredito);

            var startOfWeek = today.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            var novosClientesSemana = await _context.Clientes.CountAsync(c => c.CriadoEm >= startOfWeek);

            return new PainelDeDadosDTO
            {
                TotalReceber = totalReceber,
                Inadimplentes = inadimplentes,
                InadimplentesValor = inadimplentesValor,
                TotalClientes = totalClientes,
                NovosClientesSemana = novosClientesSemana,
                VendasHoje = vendasHoje,
                PagamentosHoje = pagamentosHoje
            };
        }

        public async Task<List<PainelDeTransacoesDTO>> ObterTransacoesRecentes()
        {
            var compras = await _context.Compras
                .Include(c => c.Cliente)
                .Where(c => c.Ativo)
                .OrderByDescending(c => c.CriadoEm)
                .Take(5)
                .Select(c => new PainelDeTransacoesDTO
                {
                    Id = c.Id,
                    Tipo = "venda",
                    Cliente = c.Cliente.Nome,
                    Valor = c.Valor,
                    Hora = c.CriadoEm.ToString("HH:mm"),
                    Data = c.CriadoEm,
                    Status = c.Ativo ? "concluido" : "estornado"
                })
                .ToListAsync();

            var pagamentos = await _context.Pagamentos
                .Include(p => p.Cliente)
                .Where(p => p.Ativo)
                .OrderByDescending(p => p.DataPagamento)
                .Take(5)
                .Select(p => new PainelDeTransacoesDTO
                {
                    Id = p.Id,
                    Tipo = "pagamento",
                    Cliente = p.Cliente.Nome,
                    Valor = p.ValorPagamento,
                    Hora = p.DataPagamento.ToString("HH:mm"),
                    Data = p.DataPagamento,
                    Status = p.Ativo ? "concluido" : "estornado"
                })
                .ToListAsync();

            var todas = compras.Cast<PainelDeTransacoesDTO>()
                .Concat(pagamentos)
                .OrderByDescending(t => t.Data)
                .Take(5)
                .ToList();

            return todas;
        }

        public async Task<List<PainelDeGraficosDTO>> ObterDadosGraficoVendas()
        {
            var trintaDiasAtras = DateTime.UtcNow.Date.AddDays(-30);
            var compras = await _context.Compras
                .Where(c => c.Ativo && c.CriadoEm >= trintaDiasAtras)
                .GroupBy(c => c.CriadoEm.Date)
                .Select(g => new { Data = g.Key, Valor = g.Sum(c => c.Valor) })
                .ToListAsync();

            var resultado = new List<PainelDeGraficosDTO>();
            for (int i = 29; i >= 0; i--)
            {
                var data = DateTime.UtcNow.Date.AddDays(-i);
                var match = compras.FirstOrDefault(c => c.Data == data);
                resultado.Add(new PainelDeGraficosDTO
                {
                    Data = data.ToString("dd/MM"),
                    Valor = match != null ? match.Valor : 0
                });
            }
            return resultado;
        }
    }
}
