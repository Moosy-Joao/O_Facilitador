using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using System.Linq;

namespace facilitador_api.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ConnectionContext _context;

    public ClienteRepository(ConnectionContext context)
    {
        _context = context;
    }

    public void Adicionar(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        _context.SaveChanges();
    }

    public Cliente? ObterPorDocumento(string documento)
    {
        return _context.Clientes
            .FirstOrDefault(c => c.Documento == documento);
    }
}