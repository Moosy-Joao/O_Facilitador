using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task Cadastrar(Cliente cliente);
        Task<Cliente?> BuscarPorId(Guid id);
        Task<Cliente?> BuscarPorDocumento(string documento);
        Task<Cliente?> BuscarPorNome(string nome);
        Task<Cliente?> BuscarPorEmail(string email);
        Task Atualizar(Cliente cliente);
        Task Desativar(Guid id);
    }
}