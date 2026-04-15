using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IClienteRepository
    {
        void Adicionar(Cliente cliente);
        Cliente? ObterPorDocumento(string documento);
    }
}