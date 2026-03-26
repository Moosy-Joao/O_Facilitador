namespace facilitador_api.Model.IReposytories
{
    public interface IPurchaseRepository
    {
        void AddPurchase(Purchase purchase);
        Purchase GetPurchase(int purchaseId);
        List<Purchase> GetAllPurchases(bool ReturnInactives = false);
        void UpdatePurchase(Purchase purchase);
        void DisablePurchase(int purchaseId);
    }
}
