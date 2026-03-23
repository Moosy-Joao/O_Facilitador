using facilitador_api.Model;

namespace facilitador_api.Repository
{
    public interface IAddressRepository
    {
        void AddAddress(Address address);
        Address GetAddress(int id);
        List<Address> GetAddresses();
        void UpdateAddress(Address address);
        void DisableAddress(int id);
    }
}
