using facilitador_api.Infrastructure.DB;
using facilitador_api.Model;
using facilitador_api.Model.IReposytories;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ConnectionContext _context = new ConnectionContext();

        public void AddClient(Client client)
        {
            try {
                _context.Clients.Add(client);
                _context.SaveChanges();
            }
            catch (Exception ex) {
                Console.WriteLine("Erro ao Salva cliente no banco: " + ex.ToString());
            }
        }

        public void DisableClient(int clientId)
        {
            try { 
                var client = _context.Clients.Find(clientId);
                
                if (client != null)
                {
                    client.Disable();
                    
                    _context.Entry(client).State = EntityState.Modified;
                    
                    _context.SaveChanges();
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Erro ao desabilitar cliente no banco: " + ex.ToString());
            }
        }

        public List<Client> GetAllClients(bool ReturnInactives = false)
        {
            try
            { 
                if (ReturnInactives)
                {
                    return _context.Clients.ToList();
                }
                
                return _context.Clients.Where(c => c.Active).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar clientes no banco: " + ex.ToString());
                return null;
            }
        }

        public Client GetClient(int clientId)
        {
            try
            { 
                return _context.Clients.Find(clientId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar cliente no banco: " + ex.ToString());
                return null;
            }
        }

        public void UpdateClient(Client client)
        {
            try
            {
                _context.Entry(client).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar cliente no banco: " + ex.ToString());
            }
        }
    }
}
