using System.ComponentModel.DataAnnotations;

namespace facilitador_api.Application.DTOs
{
    public class ClienteDTO
    {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo 'Nome' deve conter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo 'Documento' é obrigatório.")]
        public string Documento { get; set; }
    }

    public class ClienteCreateDTO : ClienteDTO
    {
        [Required(ErrorMessage = "O campo 'Saldo' é obrigatório.")]
        public decimal Saldo { get; set; }

        [Required(ErrorMessage = "O campo 'LimiteCredito' é obrigatório.")]
        public decimal LimiteCredito { get; set; }

        [Phone]
        public string Telefone { get; set; } = string.Empty;

        // Chaves estrangeiras
        [Required(ErrorMessage = "O campo 'Endereco' é obrigatório.")]
        public Guid EnderecoId { get; set; }
        [Required(ErrorMessage = "O campo 'Empresa' é obrigatório.")]
        public Guid EmpresaId { get; set; }
    }

    public class ClienteUpdateDTO
    {
        [StringLength(100, ErrorMessage = "O campo 'Nome' deve conter no máximo 100 caracteres.")]
        public string? Nome { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Documento { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? LimiteCredito { get; set; }
        [Phone]
        public string? Telefone { get; set; }
        public Guid? EnderecoId { get; set; }
        public Guid? EmpresaId { get; set; }
    }

    public class ClienteResponseDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string? Telefone { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public decimal LimiteCredito { get; set; }
        public bool Ativo { get; set; }
        public EnderecoResponseDTO? Endereco { get; set; }
        public EmpresaResponseDTO? Empresa { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime ModificadoEm { get; set; }
    }
}