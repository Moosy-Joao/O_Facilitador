using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IClienteService
    {
        bool Criar(ClienteDTO dto);
        ClienteDTO? BuscarPorId(Guid id);
        ClienteDTO? BuscarPorDocumento(string documento);
        ClienteDTO? BuscarPorEmail(string email);
        IEnumerable<ClienteDTO> BuscarPorNome(string nome);
        bool Atualizar(Guid id, ClienteDTO dto);
        bool Desativar(Guid id);
    }
}