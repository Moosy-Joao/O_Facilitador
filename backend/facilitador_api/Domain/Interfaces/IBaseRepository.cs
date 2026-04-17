namespace facilitador_api.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> BuscarPorIdAsync(Object id);
        Task<IEnumerable<T>> BuscarTodosAsync();
        Task CadastrarAsync(T entidade);
        Task AtualizarAsync(T entidade);
        Task DesativarAsync(Object id);
        Task SalvarAsync();
    }
}
