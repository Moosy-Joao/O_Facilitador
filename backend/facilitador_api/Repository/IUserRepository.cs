using facilitador_api.Model;

namespace facilitador_api.Repository
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetUser(int id);
        List<User> GetUsers();
        void UpdateUser(User user);
        void DisableUser(int id);
    }
}
