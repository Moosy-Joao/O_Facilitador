using facilitador_api.Application.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IPaymentService
    {
        string Criar(PaymentDTO dto);
    }
}