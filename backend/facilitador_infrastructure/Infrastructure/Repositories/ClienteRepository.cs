using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using facilitador_domain.Domain.DTOs;
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
            .FirstOrDefaultAsync(c => c.Documento == documento);
    }

    public async Task<Cliente?> BuscarPorEmail(string email)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
    }

    public async Task<List<Cliente>> BuscarPorNome(string nome)
    {
        return await _context.Clientes
            .Where(c => c.Nome.ToLower().Contains(nome.ToLower()))
            .ToListAsync();
    }
    public async Task<List<Cliente>> BuscarPorEmpresa(Guid empresaId)
    {
        return await _context.Clientes
            .Where(c => c.EmpresaId == empresaId)
            .ToListAsync();
    }

    // Override para incluir Endereco e Empresa
    public override async Task<List<Cliente>> BuscarTodos()
    {
        return await _context.Clientes
            .Include(c => c.Empresa)
            .ThenInclude(e => e.Endereco)
            .Include(c => c.Endereco)
            .ToListAsync();
    }

    // Implementação do método para buscar clientes inadimplentes
    public async Task<List<ClienteInadimplenteResponseDTO>> BuscarInadimplentesPorEmpresa(Guid empresaId, int diasAtraso)
    {
        var clientes = await _context.Clientes
            .Where(c => c.EmpresaId == empresaId && c.Ativo)
            .ToListAsync();

        var result = new List<ClienteInadimplenteResponseDTO>();

        foreach (var cliente in clientes)
        {
            var totalCompras = await _context.Compras
                .Where(c => c.ClienteId == cliente.Id && c.Ativo)
                .SumAsync(c => c.Valor);

            var totalPagamentos = await _context.Pagamentos
                .Where(p => p.ClienteId == cliente.Id && p.Ativo)
                .SumAsync(p => p.ValorPagamento);

            var saldoDevedor = totalCompras - totalPagamentos;
            if (saldoDevedor <= 0) continue;

            var compras = await _context.Compras
                .Where(c => c.ClienteId == cliente.Id && c.Ativo)
                .OrderBy(c => c.CriadoEm)
                .ToListAsync();

            decimal acumuladoPagamentos = totalPagamentos;
            Compra? oldestUnpaidPurchase = null;

            foreach (var compra in compras)
            {
                if (acumuladoPagamentos >= compra.Valor)
                {
                    acumuladoPagamentos -= compra.Valor;
                }
                else
                {
                    oldestUnpaidPurchase = compra;
                    break;
                }
            }

            if (oldestUnpaidPurchase == null) continue;

            var diasAtrasoCliente = (int)(DateTime.UtcNow.Date - oldestUnpaidPurchase.CriadoEm.Date).TotalDays;
            if (diasAtrasoCliente >= diasAtraso)
            {
                result.Add(new ClienteInadimplenteResponseDTO
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Documento = cliente.Documento,
                    TotalDevedor = saldoDevedor,
                    DiasAtraso = diasAtrasoCliente
                });
            }
        }

        return result;
    }

    public async Task<bool> EInadimplente(Guid clienteId, int diasAtraso = 30)
    {
        var totalCompras = await _context.Compras
            .Where(c => c.ClienteId == clienteId && c.Ativo)
            .SumAsync(c => c.Valor);

        var totalPagamentos = await _context.Pagamentos
            .Where(p => p.ClienteId == clienteId && p.Ativo)
            .SumAsync(p => p.ValorPagamento);

        var saldoDevedor = totalCompras - totalPagamentos;
        if (saldoDevedor <= 0) return false;

        var compras = await _context.Compras
            .Where(c => c.ClienteId == clienteId && c.Ativo)
            .OrderBy(c => c.CriadoEm)
            .ToListAsync();

        decimal acumuladoPagamentos = totalPagamentos;
        Compra? oldestUnpaidPurchase = null;

        foreach (var compra in compras)
        {
            if (acumuladoPagamentos >= compra.Valor)
            {
                acumuladoPagamentos -= compra.Valor;
            }
            else
            {
                oldestUnpaidPurchase = compra;
                break;
            }
        }

        if (oldestUnpaidPurchase == null) return false;

        var diasAtrasoCliente = (int)(DateTime.UtcNow.Date - oldestUnpaidPurchase.CriadoEm.Date).TotalDays;
        return diasAtrasoCliente >= diasAtraso;
    }

    public override async Task<Cliente?> BuscarPorId(Guid id)
    {
        return await _context.Clientes
            .Include(c => c.Empresa)
            .ThenInclude(e => e.Endereco)
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
