namespace facilitador_api.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> BuscarPorId(Guid id);
        Task<List<T>> BuscarTodos();
        Task<bool> Existe(Guid id);
        Task Cadastrar(T entidade);
        Task Atualizar(T entidade);
        Task Desativar(Guid id);
        Task Salvar();
    }
}
