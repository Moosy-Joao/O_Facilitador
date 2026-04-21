using facilitador_api.Application.DTOs;

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

        public Endereco()
        {
        }

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

        public Endereco(EnderecoCreateDTO dto)
        {
            Pais = dto.Pais;
            Estado = dto.Estado;
            Cidade = dto.Cidade;
            Bairro = dto.Bairro;
            Rua = dto.Rua;
            Numero = dto.Numero;
            CEP = dto.CEP;
        }

        public void AtualizarPais(string pais)
        {
            if (string.IsNullOrWhiteSpace(pais)) return;
            Pais = pais;
        }

        public void AtualizarEstado(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado)) return;
            Estado = estado;
        }

        public void AtualizarCidade(string cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade)) return;
            Cidade = cidade;
        }

        public void AtualizarBairro(string bairro)
        {
            if (string.IsNullOrWhiteSpace(bairro)) return;
            Bairro = bairro;
        }

        public void AtualizarRua(string rua)
        {
            if (string.IsNullOrWhiteSpace(rua)) return;
            Rua = rua;
        }

        public void AtualizarNumero(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero)) return;
            Numero = numero;
        }

        public void AtualizarCEP(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return;
            CEP = cep;
        }
    }
}
