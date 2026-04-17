using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ConnectionContext _context;

    public ClienteRepository(ConnectionContext context)
    {
        _context = context;
    }

    public async Task Atualizar(Cliente cliente)
    {
        var clienteExistente = _context.Clientes.Find(cliente.Id);
        if (clienteExistente == null)
        {
            throw new Exception("Cliente não encontrado.");
        }

        clienteExistente = cliente;

        _context.SaveChanges();
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

    public async Task<Cliente?> BuscarPorId(Guid id)
    {
        return await _context.Clientes
            .Include(c => c.Empresa)
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cliente?> BuscarPorNome(string nome)
    {
        return await _context.Clientes
            .Include(c => c.Empresa)
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Nome.ToLower() == nome.ToLower());
    }

    public async Task Cadastrar(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task Desativar(Guid id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
        {
            throw new Exception("Cliente não encontrado.");
        }

        if (cliente.Ativo == false)
        {
            throw new Exception("Cliente já está desativado.");
        }

        cliente.Desativar();
        await _context.SaveChangesAsync();
    }
}
