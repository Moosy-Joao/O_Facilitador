namespace facilitador_api.Model
{
    public class Client : BaseModel
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
