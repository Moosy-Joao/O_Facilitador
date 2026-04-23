using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteResponseDTO?> BuscarPorId(Guid id);
        Task<ClienteResponseDTO?> BuscarPorDocumento(string documento);
        Task<ClienteResponseDTO?> BuscarPorEmail(string email);
        Task<IEnumerable<ClienteResponseDTO>> BuscarPorNome(string nome);
        Task<bool> Criar(ClienteCreateDTO dto);
        Task<bool> Atualizar(Guid id, ClienteUpdateDTO dto);
        Task<bool> Desativar(Guid id);
    }
}