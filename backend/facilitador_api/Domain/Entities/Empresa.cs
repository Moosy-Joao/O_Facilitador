using facilitador_api.Application.DTOs;

namespace facilitador_api.Domain.Entities
{
    public class Empresa : BaseModel
    {
        public string Nome { get; private set; }
        public string CNPJ { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }
        // Relacionamento com Endereco
        public Guid EnderecoId { get; private set; }
        public Endereco Endereco { get; private set; }

        // Navegação para Clientes
        private readonly List<Cliente> _clientes = new();
        public IReadOnlyCollection<Cliente> Clientes => _clientes;

        public Empresa() { }

        public Empresa(string nome, string cnpj, string telefone, string email, Guid enderecoId)
        {
            Nome = nome;
            CNPJ = cnpj;
            Telefone = telefone;
            Email = email;
            EnderecoId = enderecoId;
        }

        public Empresa(EmpresaCreateDTO dto, Guid enderecoId)
        {
            Nome = dto.Nome;
            CNPJ = dto.CNPJ;
            Email = dto.Email;
            Telefone = dto.Telefone;
            EnderecoId = enderecoId;
        }

        // Método para adicionar um cliente à empresa
        public void AdicionarCliente(Cliente cliente) => _clientes.Add(cliente);
    }
}
