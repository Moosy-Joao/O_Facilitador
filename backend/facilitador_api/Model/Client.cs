namespace facilitador_api.Model
{
    public class Client : BaseModel
    {
        public string Name { get; private set; }
        public string CNPJ { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public decimal CreditLimit { get; private set; }
    }
}
