using facilitador_api.Domain.Entities;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories
{
    public class PagamentoRepository : BaseRepository<Pagamento>, IPagamentoRepository
    {
        public PagamentoRepository(ConnectionContext context) : base(context)
        {
        }

        public async Task<List<Pagamento>?> BuscarPorCliente(Guid clienteId)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<List<Pagamento>?> BuscarPorEmpresa(Guid empresaId)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .Where(p => p.EmpresaId == empresaId)
                .ToListAsync();
        }

        public async Task<List<Pagamento>?> BuscarPorData(DateTime pagamentoData)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .Where(p => p.DataPagamento == pagamentoData)
                .ToListAsync();
        }

        // Override para incluir Cliente e Empresa
        public override async Task<List<Pagamento>> BuscarTodos()
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .Include(p => p.Cliente)
                .ThenInclude(c => c.Endereco)
                .Include(p => p.Empresa)
                .ThenInclude(e => e.Endereco)
                .ToListAsync();
        }
    }
}
