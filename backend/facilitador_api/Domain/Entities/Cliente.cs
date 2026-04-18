using facilitador_api.Application.DTOs;

namespace facilitador_api.Domain.Entities
{
    public class Cliente : BaseModel
    {
        // Relacionamento com Empresa
        public Guid EmpresaId { get; private set; }
        public Empresa? Empresa { get; private set; }

        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Documento { get; private set; }
        public string Telefone { get; private set; }

        // Relacionamento com Endereco
        public Guid EnderecoId { get; private set; }
        public Endereco? Endereco { get; private set; }
        public decimal Saldo { get; private set; }
        public decimal LimiteCredito { get; private set; }
        public float? Nota { get; private set; }

        public Cliente(Guid empresaId, string nome, string email, string documento, string telefone, Guid enderecoId, decimal saldo, decimal limiteCredito)
        {
            EmpresaId = empresaId;
            Nome = nome;
            Email = email;
            Documento = documento;
            Telefone = telefone;
            EnderecoId = enderecoId;
            Saldo = saldo;
            LimiteCredito = limiteCredito;
        }

        public Cliente(ClienteCreateDTO dto, Guid empresaId, Guid enderecoId)
        {
            EmpresaId = dto.Empresa;
            Nome = dto.Nome;
            Email = dto.Email;
            Documento = dto.Documento;
            Telefone = dto.Telefone;
            EnderecoId = enderecoId;
            Saldo = dto.Saldo;
            LimiteCredito = dto.LimiteCredito;
            EnderecoId = enderecoId;
        }
    }
}