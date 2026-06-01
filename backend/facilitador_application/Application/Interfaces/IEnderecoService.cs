using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEnderecoService
    {
        Task<EnderecoResponseDTO?> BuscarPorId(Guid id);
        Task<EnderecoResponseDTO?> BuscarPorCEP(string cep);
        Task<List<EnderecoResponseDTO>?> BuscarEnderecos();
        Task<EnderecoResponseDTO?> Criar(EnderecoCreateDTO dto);
        Task<bool> Atualizar(Guid id, EnderecoUpdateDTO dto);
        Task<bool> Ativar(Guid id);
        Task<bool> Desativar(Guid id);
    }
}
