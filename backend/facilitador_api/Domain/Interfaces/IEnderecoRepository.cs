using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IEnderecoRepository : IBaseRepository<Endereco>
    {
        Task<Endereco?> BuscarPorCEP(string CEP);
    }
}
