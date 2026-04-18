using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEnderecoService
    {
        Task<EnderecoDTO?> BuscarPorId(Guid id);
        Task<EnderecoDTO?> BuscarPorCEP(string cep);
        Task<bool> Criar(EnderecoDTO dto);
        Task<bool> Atualizar(Guid id, EnderecoDTO dto);
        Task<bool> Desativar(Guid id);
    }
}
