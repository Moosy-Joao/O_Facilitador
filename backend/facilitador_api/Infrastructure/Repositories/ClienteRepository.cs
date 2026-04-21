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
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Documento == documento);
    }

    public async Task<Cliente?> BuscarPorEmail(string email)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<List<Cliente>> BuscarPorNome(string nome)
    {
        return await _context.Clientes
            .AsNoTracking()
            .Where(c => c.Nome.ToLower().Contains(nome.ToLower()))
            .ToListAsync();
    }

    // Override para incluir Endereco e Empresa
    public override async Task<List<Cliente>> BuscarTodos()
    {
        return await _context.Clientes
            .AsNoTracking()
            //.Include(c => c.Empresa)
            //.ThenInclude(e => e.Endereco)
            //.Include(c => c.Endereco)
            .ToListAsync();
    }
}
