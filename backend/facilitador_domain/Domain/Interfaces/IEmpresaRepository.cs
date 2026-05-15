using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IEmpresaRepository : IBaseRepository<Empresa>
    {
        Task<List<Empresa>?> BuscarPorNome(string nome);
        Task<Empresa?> BuscarPorCNPJ(string cnpj);
    }
}
