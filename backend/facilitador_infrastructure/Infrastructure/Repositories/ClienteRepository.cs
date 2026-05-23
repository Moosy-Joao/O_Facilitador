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
    public async Task<List<Cliente>> BuscarPorEmpresa(Guid empresaId)
    {
        return await _context.Clientes
            .AsNoTracking()
            .Where(c => c.EmpresaId == empresaId)
            .ToListAsync();
    }

    // Override para incluir Endereco e Empresa
    public override async Task<List<Cliente>> BuscarTodos()
    {
        return await _context.Clientes
            .AsNoTracking()
            .Include(c => c.Empresa)
            .ThenInclude(e => e.Endereco)
            .Include(c => c.Endereco)
            .ToListAsync();
    }

    // Implementação do método para buscar clientes inadimplentes
    public async Task<List<ClienteInadimplenteResponseDTO>> BuscarInadimplentesPorEmpresa(Guid empresaId, int diasAtraso)
    {
        var dataLimite = DateTime.UtcNow.Date.AddDays(-diasAtraso);

        var query = from cliente in _context.Clientes
                    where cliente.EmpresaId == empresaId && cliente.Ativo
                    let totalCompras = _context.Compras
                        .Where(c => c.ClienteId == cliente.Id && c.Ativo)
                        .Sum(c => c.Valor)
                    let totalPagamentos = _context.Pagamentos
                        .Where(p => p.ClienteId == cliente.Id && p.Ativo)
                        .Sum(p => p.ValorPagamento)
                    let saldoDevedor = totalCompras - totalPagamentos
                    let primeiraCompra = _context.Compras
                        .Where(c => c.ClienteId == cliente.Id && c.Ativo)
                        .OrderBy(c => c.CriadoEm)
                        .FirstOrDefault()
                    let diasAtrasoCliente = primeiraCompra != null && saldoDevedor > 0
                        ? (int)(DateTime.UtcNow.Date - primeiraCompra.CriadoEm.Date).TotalDays
                        : 0
                    where saldoDevedor > 0 && diasAtrasoCliente >= diasAtraso
                    select new ClienteInadimplenteResponseDTO
                    {
                        Id = cliente.Id,
                        Nome = cliente.Nome,
                        Documento = cliente.Documento,
                        TotalDevedor = saldoDevedor,
                        DiasAtraso = diasAtrasoCliente
                    };

        return await query.ToListAsync();
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

        var compraMaisAntiga = await _context.Compras
            .Where(c => c.ClienteId == clienteId && c.Ativo)
            .OrderBy(c => c.CriadoEm)
            .FirstOrDefaultAsync();

        if (compraMaisAntiga == null) return false;

        var diasAtrasoCliente = (int)(DateTime.UtcNow.Date - compraMaisAntiga.CriadoEm.Date).TotalDays;
        return diasAtrasoCliente >= diasAtraso;
    }
}
