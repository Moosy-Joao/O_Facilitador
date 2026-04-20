using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<Cliente?> BuscarPorDocumento(string documento);
        Task<IEnumerable<Cliente>> BuscarPorNome(string nome);
        Task<Cliente?> BuscarPorEmail(string email);
    }
}