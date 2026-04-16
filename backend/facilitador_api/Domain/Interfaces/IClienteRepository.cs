using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IClienteRepository
    {
        void Cadastrar(Cliente cliente);
        Cliente? BuscarPorId(Guid id);
        Cliente? BuscarPorDocumento(string documento);
        Cliente? BuscarPorNome(string nome);
        Cliente? BuscarPorEmail(string email);
        void Atualizar(Cliente cliente);
        void Desativar(Guid id);
    }
}