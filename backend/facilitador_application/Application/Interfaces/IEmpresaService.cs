using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEmpresaService
    {
        Task<EmpresaResponseDTO?> BuscarPorId(Guid id);
        Task<EmpresaResponseDTO?> BuscarPorCNPJ(string cnpj);
        Task<List<EmpresaResponseDTO>?> BuscarPorNome(string nome);
        Task<List<EmpresaResponseDTO>?> BuscarEmpresas();
        Task<bool> Criar(EmpresaCreateDTO dto);
        Task<bool> Atualizar(Guid id, EmpresaUpdateDTO dto);
        Task<bool> Desativar(Guid id);
    }
}
