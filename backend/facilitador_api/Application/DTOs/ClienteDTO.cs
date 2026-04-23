using System.ComponentModel.DataAnnotations;

namespace facilitador_api.Application.DTOs
{
    public class BaseClienteDTO
    {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo 'Nome' deve conter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Documento' é obrigatório.")]
        public string Documento { get; set; } = string.Empty;
    }

    public class ClienteDTO : BaseClienteDTO
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime ModificadoEm { get; set; }
    }

    public class ClienteCreateDTO : BaseClienteDTO
    {
        [Required(ErrorMessage = "O campo 'Saldo' é obrigatório.")]
        public decimal Saldo { get; set; } = 0;

        [Required(ErrorMessage = "O campo 'LimiteCredito' é obrigatório.")]
        public decimal LimiteCredito { get; set; } = 0;

        [Phone]
        public string Telefone { get; set; } = string.Empty;

        // Chaves estrangeiras
        [Required(ErrorMessage = "O campo 'Endereco' é obrigatório.")]
        public Guid Endereco { get; set; }
        [Required(ErrorMessage = "O campo 'Empresa' é obrigatório.")]
        public Guid Empresa { get; set; }
    }

    public class ClienteUpdateDTO : BaseClienteDTO
    {
        [Phone]
        public string? Telefone { get; set; } = null;

        [Required(ErrorMessage = "O campo 'LimiteCredito' é obrigatório.")]
        public decimal? LimiteCredito { get; set; } = null;

        // Chaves estrangeiras
        public Guid? Endereco { get; set; } = null;
    }

    public class ClienteResponseDTO : ClienteDTO
    {
        public decimal Saldo { get; set; }

        public decimal LimiteCredito { get; set; }

        public string? Telefone { get; set; }

        public float Nota { get; set; }

        public EnderecoDTO? Endereco { get; set; }
        public EmpresaDTO? Empresa { get; set; }
    }
}