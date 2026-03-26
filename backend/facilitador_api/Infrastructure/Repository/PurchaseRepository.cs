using facilitador_api.Infrastructure.DB;
using facilitador_api.Model;
using facilitador_api.Model.IReposytories;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ConnectionContext _context = new ConnectionContext();

        public void AddPurchase(Purchase purchase)
        {
            try
            {
                _context.Purchases.Add(purchase);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar compra no banco: " + ex.Message);
            }
        }

        public void DisablePurchase(int purchaseId)
        {
            try
            {
                var purchase = _context.Purchases.Find(purchaseId);
               
                if (purchase != null)
                {
                    purchase.Disable();
                  
                    _context.Entry(purchase).State = EntityState.Modified;
                    
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao desabilitar compra no banco: " + ex.Message);
            }
        }

        public List<Purchase> GetAllPurchases(bool ReturnInactives = false)
        {
            try
            {
                if (ReturnInactives)
                {
                    return _context.Purchases.ToList();
                }
                else
                {
                    return _context.Purchases.Where(p => p.Active).ToList();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter compras do banco: " + ex.Message);
                return null;
            }
        }

        public Purchase GetPurchase(int purchaseId)
        {
            try
            {
                return _context.Purchases.Find(purchaseId);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter compra do banco: " + ex.Message);
                return null;
            }
        }

        public void UpdatePurchase(Purchase purchase)
        {
            try
            {
                _context.Entry(purchase).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar compra no banco: " + ex.Message);
            }
        }
    }
}
