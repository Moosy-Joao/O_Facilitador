using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario?> BuscarPorEmail(string email);
        Task<List<Usuario>> BuscarPorNome(string nome);
        Task<List<Usuario>> BuscarPorEmpresa(Guid empresaId);
    }
}
