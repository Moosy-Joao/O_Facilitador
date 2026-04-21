using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories
{
    public class EmpresaRepository : BaseRepository<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(ConnectionContext context) : base(context)
        {
        }

        public async Task<Empresa?> BuscarPorCNPJ(string cnpj)
        {
            var empresa = await _context.Empresas
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(e => e.CNPJ == cnpj);

            return empresa;
        }

        public async Task<List<Empresa>?> BuscarPorNome(string nome)
        {
            var empresas = await _context.Empresas
                .Include(e => e.Endereco)
                .Where(e => e.Nome.ToLower() == nome.ToLower())
                .ToListAsync();

            return empresas;
        }

        // Override para incluir Endereco
        public override async Task<List<Empresa>> BuscarTodos()
        {
            var empresas = await _context.Empresas
                .Include(e => e.Endereco)
                .ToListAsync();
            return empresas;
        }
    }
}
