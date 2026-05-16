using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteResponseDTO?> BuscarPorId(Guid id);
        Task<ClienteResponseDTO?> BuscarPorDocumento(string documento);
        Task<ClienteResponseDTO?> BuscarPorEmail(string email);
        Task<List<ClienteResponseDTO>> BuscarPorNome(string nome);
        Task<List<ClienteResponseDTO>> BuscarClientes();
        Task<bool> Criar(ClienteCreateDTO dto);
        Task<bool> Atualizar(Guid id, ClienteUpdateDTO dto);
        Task<bool> Ativar(Guid id);
        Task<bool> Desativar(Guid id);
    }
}