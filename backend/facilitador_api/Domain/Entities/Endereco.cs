namespace facilitador_api.Domain.Entities
{
    public class Endereco : BaseModel
    {
        public string Pais { get; private set; }
        public string Estado { get; private set; }
        public string Cidade { get; private set; }
        public string Bairro { get; private set; }
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string CEP { get; private set; }

        public Endereco(string pais, string estado, string cidade, string bairro, string rua, string numero, string cep)
        {
            Pais = pais;
            Estado = estado;
            Cidade = cidade;
            Bairro = bairro;
            Rua = rua;
            Numero = numero;
            CEP = cep;
        }
    }
}
