using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IPagamentoRepository : IBaseRepository<Pagamento>
    {
        Task<List<Pagamento>> BuscarPorData(DateTime dataPagamento);
        Task<List<Pagamento>> BuscarPorEmpresa(Guid empresaId);
        Task<List<Pagamento>> BuscarPorCliente(Guid clienteId);
    }
}