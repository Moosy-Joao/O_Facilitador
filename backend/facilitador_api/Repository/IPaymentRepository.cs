using facilitador_api.Model;

namespace facilitador_api.Repository
{
    public interface IPaymentRepository
    {
        void AddPayment(Payment payment);
        Payment GetPayment(int id);
        List<Payment> GetAllPayments();
        void UpdatePayment(Payment payment);
        void DisablePayment(int id);
    }
}
