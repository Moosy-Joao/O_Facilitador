using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEmpresaService
    {
        Task<EmpresaDTO?> BuscarPorId(Guid id);
        Task<EmpresaDTO?> BuscarPorCNPJ(string cnpj);
        Task<IEnumerable<EmpresaDTO>> BuscarPorNome(string nome);
        Task<bool> Criar(EmpresaDTO dto);
        Task<bool> Atualizar(Guid id, EmpresaDTO dto);
        Task<bool> Desativar(Guid id);
    }
}
