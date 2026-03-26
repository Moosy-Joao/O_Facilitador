namespace facilitador_api.Model.IReposytories
{
    public interface IAddressRepository
    {
        void AddAddress(Address address);
        Address GetAddress(int addressId);
        List<Address> GetAddresses(bool ReturnInactives = false);
        void UpdateAddress(Address address);
        void DisableAddress(int id);
    }
}
