using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface ICompraService
    {
        Task<List<CompraResponseDTO>> BuscarCompras();
        Task<CompraResponseDTO?> BuscarPorId(Guid id);
        Task<List<CompraResponseDTO>> BuscarPorCliente(Guid clienteId);
        Task<List<CompraResponseDTO>> BuscarPorEmpresa(Guid empresaId);

        Task<bool> Criar(CompraCreateDTO dto);
        Task<bool> Atualizar(Guid id, CompraUpdateDTO dto);
        Task<bool> Desativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}