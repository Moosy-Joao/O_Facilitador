using facilitador_api.Domain.Entities;

namespace facilitador_api.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        void Adicionar(Payment Payment);
        Payment? ObterPorDocumento(string documento);
    }
}