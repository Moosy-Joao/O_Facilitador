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

    public void Atualizar(Cliente cliente)
    {
        try
        {
            var clienteExistente = _context.Clientes.Find(cliente.Id);
            if (clienteExistente == null)
            {
                throw new Exception("Cliente não encontrado.");
            }

            clienteExistente = cliente;

            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar o cliente: " + ex.Message);
        }
    }

    public Cliente? BuscarPorDocumento(string documento)
    {
        try
        {
            return _context.Clientes
                .Include(c => c.Empresa)
                .Include(c => c.Endereco)
                .FirstOrDefault(c => c.Documento == documento);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao buscar cliente por documento: " + ex.Message);
        }
    }

    public Cliente? BuscarPorEmail(string email)
    {
        try
        {
            return _context.Clientes
                .Include(c => c.Empresa)
                .Include(c => c.Endereco)
                .FirstOrDefault(c => c.Email == email);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao buscar cliente por email: " + ex.Message);
        }
    }

    public Cliente? BuscarPorId(Guid id)
    {
        try
        {
            return _context.Clientes
                .Include(c => c.Empresa)
                .Include(c => c.Endereco)
                .FirstOrDefault(c => c.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao buscar cliente por ID: " + ex.Message);
        }
    }

    public Cliente? BuscarPorNome(string nome)
    {
        try
        {
            return _context.Clientes
                .Include(c => c.Empresa)
                .Include(c => c.Endereco)
                .FirstOrDefault(c => c.Nome.ToLower() == nome.ToLower());
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao buscar cliente por nome: " + ex.Message);
        }
    }

    public void Cadastrar(Cliente cliente)
    {
        try
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao cadastrar cliente: " + ex.Message);
        }
    }

    public void Desativar(Guid id)
    {
        try
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado.");
            }

            if (cliente.Ativo == false)
            {
                throw new Exception("Cliente já está desativado.");
            }

            cliente.Desativar();
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao desativar cliente: " + ex.Message);
        }
    }
}
