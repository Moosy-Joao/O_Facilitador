namespace facilitador_api.Model
{
    public class Client : BaseModel
    {
        private string Name { get; set; }
        private string CNPJ { get; set; }
        private string Phone { get; set; }
        private string Address { get; set; }
        private decimal CreditLimit { get; set; }
    }
}
