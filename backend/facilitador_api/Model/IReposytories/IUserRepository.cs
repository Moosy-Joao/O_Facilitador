namespace facilitador_api.Model.IReposytories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetUser(int userId);
        List<User> GetUsers(bool ReturnInactives = false);
        void UpdateUser(User user);
        void DisableUser(int id);
    }
}
