using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories;

public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(ConnectionContext context) : base(context)
    {
    }

    public async Task<Cliente?> BuscarPorDocumento(string documento)
    {
        return await _context.Clientes
            .Include(c => c.Empresa)
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Documento == documento);
    }

    public async Task<Cliente?> BuscarPorEmail(string email)
    {
        return await _context.Clientes
            .Include(c => c.Empresa)
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Cliente>> BuscarPorNome(string nome)
    {
        return await _context.Clientes
            .Include(c => c.Endereco)
            .Include(c => c.Empresa)
            .Where(c => c.Nome.ToLower().Contains(nome.ToLower()))
            .ToListAsync();
    }
}
