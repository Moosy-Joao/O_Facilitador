using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IEnderecoRepository
    {
        void Cadastrar(Endereco endereco);
        Endereco? BuscarPorId(Guid id);
        Endereco BuscarPorCEP(string CEP);
        void Atualizar(Endereco endereco);
        void Desativar(Guid id);
    }
}
