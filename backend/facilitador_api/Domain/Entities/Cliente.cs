namespace facilitador_api.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Nome { get; private set; }
        public string Documento { get; private set; }
        public string Telefone { get; private set; }

        public string? Endereco { get; private set; }
        public decimal LimiteCredito { get; private set; } = 0;
        public bool Ativo { get; private set; } = true;
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

        public Cliente(string nome, string documento, string telefone)
        {
            Nome = nome;
            Documento = documento;
            Telefone = telefone;
        }
    }
}