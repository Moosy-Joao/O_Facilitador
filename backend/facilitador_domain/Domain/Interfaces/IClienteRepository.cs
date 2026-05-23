using facilitador_api.Domain.Entities;
using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Domain.Interfaces
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<Cliente?> BuscarPorDocumento(string documento);
        Task<List<Cliente>> BuscarPorNome(string nome);
        Task<Cliente?> BuscarPorEmail(string email);
        Task<List<Cliente>> BuscarPorEmpresa(Guid empresaId);
        Task<List<ClienteInadimplenteResponseDTO>> BuscarInadimplentesPorEmpresa(Guid empresaId, int diasAtraso);
        Task<bool> EInadimplente(Guid clienteId, int diasAtraso = 30);
    }
}