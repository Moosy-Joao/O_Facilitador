using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IEmpresaRepository
    {
        void Cadastrar(Empresa empresa);
        Empresa? BuscarPorId(Guid id);
        Empresa? BuscarPorNome(string nome);
        void Atualizar(Empresa empresa);
        void Desativar(Guid id);
    }
}
