using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IEmpresaService
    {
        bool Criar(EmpresaDTO dto);
        EmpresaDTO? BuscarPorId(Guid id);
        EmpresaDTO? BuscarPorCNPJ(string cnpj);
        IEnumerable<EmpresaDTO> BuscarPorNome(string nome);
        bool Atualizar(Guid id, EmpresaDTO dto);
        bool Desativar(Guid id);
    }
}
