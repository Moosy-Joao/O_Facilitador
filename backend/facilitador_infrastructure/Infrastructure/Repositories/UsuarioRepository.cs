using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ConnectionContext context) : base(context)
        {
        }

        public async Task<Usuario?> BuscarPorEmail(string email)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<List<Usuario>> BuscarPorEmpresa(Guid empresaId)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(c => c.EmpresaId == empresaId)
                .ToListAsync();
        }

        public async Task<List<Usuario>> BuscarPorNome(string nome)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(c => c.Nome.ToLower().Contains(nome.ToLower()))
                .ToListAsync();
        }

        // Override para incluir Empresa
        public override async Task<List<Usuario>> BuscarTodos()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Include(c => c.Empresa)
                .ToListAsync();
        }
    }
}
