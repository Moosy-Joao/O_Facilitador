using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEnderecoService
    {
        string Criar(EnderecoDTO dto);
        EnderecoDTO? BuscarPorId(Guid id);
        EnderecoDTO? BuscarPorCEP(string cep);
        string Atualizar(Guid id, EnderecoDTO dto);
        string Desativar(Guid id);
    }
}
