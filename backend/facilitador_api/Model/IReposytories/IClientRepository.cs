namespace facilitador_api.Model.IReposytories
{
    public interface IClientRepository
    {
        void AddClient(Client client);
        Client GetClient(int clientId);
        List<Client> GetAllClients(bool ReturnInactives = false);
        void UpdateClient(Client client);
        void DisableClient(int id);
    }
}
