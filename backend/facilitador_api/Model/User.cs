namespace facilitador_api.Model
{
    public class User : BaseModel
    {
        private string Name { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string Role { get; set; }
    }
}
