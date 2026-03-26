namespace facilitador_api.Model.IReposytories
{
    public interface IPaymentRepository
    {
        void AddPayment(Payment payment);
        Payment GetPayment(int paymentId);
        List<Payment> GetAllPayments(bool ReturnInactives = false);
        void UpdatePayment(Payment payment);
        void DisablePayment(int id);
    }
}
