using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IClienteService
    {
        string Criar(ClienteDTO dto);
    }
}