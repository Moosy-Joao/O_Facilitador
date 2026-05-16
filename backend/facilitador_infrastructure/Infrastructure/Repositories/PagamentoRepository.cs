using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories
{
    public class PagamentoRepository : BaseRepository<Pagamento>, IPagamentoRepository
    {
        public PagamentoRepository(ConnectionContext context) : base(context)
        {
        }

        public async Task<List<Pagamento>> BuscarPorData(DateTime dataPagamento)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .Where(p => p.DataPagamento.Date == dataPagamento.Date)
                .ToListAsync();
        }

        public async Task<List<Pagamento>> BuscarPorEmpresa(Guid empresaId)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .Where(p => p.EmpresaId == empresaId)
                .ToListAsync();
        }

        public async Task<List<Pagamento>> BuscarPorCliente(Guid clienteId)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }
    }
}