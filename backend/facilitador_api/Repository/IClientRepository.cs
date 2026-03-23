using facilitador_api.Model;

namespace facilitador_api.Repository
{
    public interface IClientRepository
    {
        void AddClient(Client client);
        Client GetClient(int id);
        List<Client> GetAllClients();
        void UpdateClient(Client client);
        void DisableClient(int id);
    }
}
