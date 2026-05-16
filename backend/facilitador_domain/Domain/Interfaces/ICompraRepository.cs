using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface ICompraRepository : IBaseRepository<Compra>
    {
        Task<List<Compra>> BuscarPorEmpresa(Guid empresaId);
        Task<List<Compra>> BuscarPorCliente(Guid clienteId);
    }
}