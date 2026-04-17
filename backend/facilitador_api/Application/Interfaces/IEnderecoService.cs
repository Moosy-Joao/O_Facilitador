using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEnderecoService
    {
        bool Criar(EnderecoDTO dto);
        EnderecoDTO? BuscarPorId(Guid id);
        EnderecoDTO? BuscarPorCEP(string cep);
        bool Atualizar(Guid id, EnderecoDTO dto);
        bool Desativar(Guid id);
    }
}
