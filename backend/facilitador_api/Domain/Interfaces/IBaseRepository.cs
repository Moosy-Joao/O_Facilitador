namespace facilitador_api.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> BuscarPorId(Object id);
        Task<IEnumerable<T>> BuscarTodos();
        Task Cadastrar(T entidade);
        Task Atualizar(T entidade);
        Task Desativar(Object id);
        Task Salvar();
    }
}
