using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories
{
    public class CompraRepository : BaseRepository<Compra>, ICompraRepository
    {
        public CompraRepository(ConnectionContext context) : base(context)
        {
        }

        public async Task<List<Compra>> BuscarPorEmpresa(Guid empresaId)
        {
            return await _context.Compras
                .AsNoTracking()
                .Where(c => c.EmpresaId == empresaId)
                .ToListAsync();
        }

        public async Task<List<Compra>> BuscarPorCliente(Guid clienteId)
        {
            return await _context.Compras
                .AsNoTracking()
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();
        }
    }
}