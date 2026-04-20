using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEmpresaService
    {
        Task<EmpresaResponseDTO?> BuscarPorId(Guid id);
        Task<EmpresaResponseDTO?> BuscarPorCNPJ(string cnpj);
        Task<IEnumerable<EmpresaResponseDTO>?> BuscarPorNome(string nome);
        Task<bool> Criar(EmpresaCreateDTO dto);
        Task<bool> Atualizar(Guid id, EmpresaUpdateDTO dto);
        Task<bool> Desativar(Guid id);
    }
}
