using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        protected readonly ConnectionContext _context = null;
        protected readonly DbSet<T> _dbSet = null;

        public BaseRepository(ConnectionContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public Task AtualizarAsync(T entidade)
        {
            _dbSet.Update(entidade);
            return Task.CompletedTask;
        }

        public async Task<T> BuscarPorIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> BuscarTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task CadastrarAsync(T entidade)
        {
            await _dbSet.AddAsync(entidade);
        }

        public async Task DesativarAsync(object id)
        {
            var entidadeExistente = await _dbSet.FindAsync(id);
            if (entidadeExistente == null)
            {
                throw new Exception("Entidade não encontrada.");
            }

            if (entidadeExistente.Ativo == false)
            {
                throw new Exception("Entidade já está desativada.");
            }

            entidadeExistente.Desativar();
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
