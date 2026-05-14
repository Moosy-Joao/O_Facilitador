using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioResponseDTO>> BuscarUsuarios();
        Task<UsuarioResponseDTO?> BuscarPorId(Guid id);
        Task<bool> Criar(UsuarioCreateDTO dto);
        Task<bool> Atualizar(Guid id, UsuarioUpdateDTO dto);
        Task<bool> Ativar(Guid id);
        Task<bool> Desativar(Guid id);
    }
}
