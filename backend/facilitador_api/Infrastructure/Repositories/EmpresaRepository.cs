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

        public async Task<Empresa?> BuscarPorId(Guid id)
        {
            var empresa = _context.Empresas
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (empresa == null)
            {
                throw new Exception("Empresa não encontrada.");
            }

            return await empresa;
        }

        public async Task<Empresa?> BuscarPorNome(string nome)
        {
            var empresa = _context.Empresas
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(e => e.Nome.ToLower() == nome.ToLower());

            if (empresa == null)
            {
                throw new Exception("Empresa não encontrada.");
            }

            return await empresa;
        }
    }
}
