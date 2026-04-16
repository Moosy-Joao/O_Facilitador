using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEmpresaService
    {
        string Criar(EmpresaDTO dto);
        EmpresaDTO? BuscarPorId(Guid id);
        EmpresaDTO? BuscarPorCNPJ(string cnpj);
        IEnumerable<EmpresaDTO> BuscarPorNome(string nome);
        string Atualizar(Guid id, EmpresaDTO dto);
        string Desativar(Guid id);
    }
}
