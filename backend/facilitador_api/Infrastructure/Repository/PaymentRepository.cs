using facilitador_api.Infrastructure.DB;
using facilitador_api.Model;
using facilitador_api.Model.IReposytories;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ConnectionContext _context = new ConnectionContext();

        public void AddPayment(Payment payment)
        {
            try
            {
                _context.Payments.Add(payment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar pagamento no banco: " + ex.ToString());
            }
        }

        public void DisablePayment(int paymentId)
        {
            try
            {
                var payment = _context.Payments.Find(paymentId);

                if (payment != null)
                {
                    payment.Disable();
                    
                    _context.Entry(payment).State = EntityState.Modified;

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao desabilitar pagamento no banco: " + ex.ToString());
            }
        }

        public List<Payment> GetAllPayments(bool ReturnInactives = false)
        {
            try
            {
                if (ReturnInactives)
                {
                    return _context.Payments.ToList();
                }
                return _context.Payments.Where(p => p.Active).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar pagamentos no banco: " + ex.ToString());
                return null;
            }
        }

        public Payment GetPayment(int paymentId)
        {
            try
            {
                return _context.Payments.Find(paymentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar pagamento no banco: " + ex.ToString());
                return null;
            }
        }

        public void UpdatePayment(Payment payment)
        {
            try
            {
                _context.Entry(payment).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar pagamento no banco: " + ex.ToString());
            }
        }
    }
}
