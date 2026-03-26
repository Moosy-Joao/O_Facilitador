using facilitador_api.Infrastructure.DB;
using facilitador_api.Model;
using facilitador_api.Model.IReposytories;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ConnectionContext _context = new ConnectionContext();

        public void AddAddress(Address address)
        {
            try {
                _context.Addresses.Add(address);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar endereco no banco: " + ex.ToString());
            }
        }

        public void DisableAddress(int addressId)
        {
            try
            {
                var address = _context.Addresses.Find(addressId);
                
                if (address != null)
                {
                    address.Disable();
                
                    _context.Entry(address).State = EntityState.Modified;
                    
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao desabilitar endereco no banco: " + ex.ToString());
            }
        }

        public List<Address> GetAddresses(bool ReturnInactives = false)
        {
            try
            {
                if (ReturnInactives)
                {
                    return _context.Addresses.ToList();
                }

                return _context.Addresses.Where(a => a.Active).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar enderecos no banco: " + ex.ToString());
                return null;
            }
        }

        public Address GetAddress(int addressId)
        {
            try 
            { 
                return _context.Addresses.Find(addressId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar endereco no banco: " + ex.ToString());
                return null;
            }
        }

        public void UpdateAddress(Address address)
        {
            try
            {
                _context.Entry(address).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar endereco no banco: " + ex.ToString());
            }
        }
    }
}
