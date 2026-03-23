using facilitador_api.Model;

namespace facilitador_api.Repository
{
    public interface IPurchaseRepository
    {
        void AddPurchase(Purchase purchase);
        Purchase GetPurchase(int purchaseId);
        List<Purchase> GetAllPurchases();
        void UpdatePurchase(Purchase purchase);
        void DisablePurchase(int purchaseId);
    }
}
