namespace facilitador_api.Model
{
    public class Client : BaseModel
    {
        public string ?Name { get; private set; }
        public string ?CNPJ { get; private set; }
        public string ?Phone { get; private set; }
        public string ?Address { get; private set; }
        public decimal CreditLimit { get; private set; }

        private Client()
        {
            Name = string.Empty;
            CNPJ = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
            CreditLimit = 0;
        }

        public Client(string name, string cnpj, string phone)
        {
            Name = name;
            CNPJ = cnpj;
            Phone = phone;
            Address = string.Empty;
            CreditLimit = 0;
        }

    }
}