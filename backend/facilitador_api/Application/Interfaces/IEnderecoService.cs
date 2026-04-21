using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEnderecoService
    {
        Task<EnderecoResponseDTO?> BuscarPorId(Guid id);
        Task<EnderecoResponseDTO?> BuscarPorCEP(string cep);
        Task<bool> Criar(EnderecoCreateDTO dto);
        Task<bool> Atualizar(Guid id, EnderecoUpdateDTO dto);
        Task<bool> Desativar(Guid id);
    }
}
