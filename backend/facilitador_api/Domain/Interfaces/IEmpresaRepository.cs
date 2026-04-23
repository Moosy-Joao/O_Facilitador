using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IEmpresaRepository : IBaseRepository<Empresa>
    {
        Task<Empresa?> BuscarPorId(Guid id);
        Task<Empresa?> BuscarPorNome(string nome);
    }
}
