using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IPagamentoService
    {
        Task<List<PagamentoResponseDTO>> BuscarPagamentos();
        Task<PagamentoResponseDTO?> BuscarPorId(Guid id);
        Task<List<PagamentoResponseDTO>> BuscarPorCliente(Guid clienteId);
        Task<List<PagamentoResponseDTO>> BuscarPorEmpresa(Guid empresaId);
        Task<List<PagamentoResponseDTO>> BuscarPorData(DateTime dataPagamento);

        Task<bool> Criar(PagamentoCreateDTO dto);
        Task<bool> Atualizar(Guid id, PagamentoUpdateDTO dto);
        Task<bool> Desativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}