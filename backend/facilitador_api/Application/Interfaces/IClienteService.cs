using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IClienteService
    {
        string Criar(ClienteDTO dto);
        ClienteDTO? BuscarPorId(Guid id);
        ClienteDTO? BuscarPorDocumento(string documento);
        ClienteDTO? BuscarPorEmail(string email);
        IEnumerable<ClienteDTO> BuscarPorNome(string nome);
        string Atualizar(Guid id, ClienteDTO dto);
        string Desativar(Guid id);
    }
}