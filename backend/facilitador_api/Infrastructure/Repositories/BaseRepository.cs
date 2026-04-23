using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        protected readonly ConnectionContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ConnectionContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public Task Atualizar(T entidade)
        {
            _dbSet.Update(entidade);
            return Task.CompletedTask;
        }

        public async Task<T?> BuscarPorId(Guid id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async virtual Task<List<T>> BuscarTodos()
        {
            return await _dbSet
                .ToListAsync();
        }

        public async Task Cadastrar(T entidade)
        {
            await _dbSet.AddAsync(entidade);
        }

        public async Task Desativar(Guid id)
        {
            var entidadeExistente = await _dbSet.FindAsync(id);
            if (entidadeExistente == null)
            {
                return;
            }

            if (entidadeExistente.Ativo == false)
            {
                throw new Exception("Entidade já está desativada.");
            }

            entidadeExistente.Desativar();
        }

        public async Task Ativar(Guid id)
        {
            var entidadeExistente = await _dbSet.FindAsync(id);
            if (entidadeExistente == null)
            {
                return;
            }

            if (entidadeExistente.Ativo == true)
            {
                throw new Exception("Entidade já está Ativada.");
            }

            entidadeExistente.Ativar();
        }

        public async Task<bool> Existe(Guid id)
        {
            return await _dbSet.AsNoTracking().AnyAsync(e => e.Id == id);
        }

        public Task Salvar()
        {
            return _context.SaveChangesAsync();
        }
    }
}
