namespace facilitador_api.Model
{
    public class User : BaseModel
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }
    }
}
