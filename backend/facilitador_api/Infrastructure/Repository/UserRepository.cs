using facilitador_api.Infrastructure.DB;
using facilitador_api.Model;
using facilitador_api.Model.IReposytories;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionContext _context = new ConnectionContext();

        public void AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Erro ao salvar usuario no banco: " + ex.ToString());
            }
        }

        public void DisableUser(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);

                if (user != null)
                {
                    user.Disable();

                    _context.Entry(user).State = EntityState.Modified;

                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao desabilitar usuario no banco: " + ex.ToString());
            }
        }

        public User GetUser(int userId)
        {
            try
            {
                return _context.Users.Find(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar usuario no banco: " + ex.ToString());
                return null;
            }
        }

        public List<User> GetUsers(bool ReturnInactives = false)
        {
            try
            {
                if (ReturnInactives)
                {
                    return _context.Users.ToList();
                }

                return _context.Users.Where(u => u.Active).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar usuarios no banco: " + ex.ToString());
                return null;
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar usuario no banco: " + ex.ToString());
            }
        }
    }
}
