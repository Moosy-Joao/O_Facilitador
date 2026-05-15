using System.ComponentModel.DataAnnotations;

namespace facilitador_domain.Domain.DTOs
{
    public class EmpresaDTO
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo Nome deve conter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo CNPJ é obrigatório.")]
        public string CNPJ { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Email { get; set; } = string.Empty;
    }

    public class EmpresaCreateDTO : EmpresaDTO
    {
        [Phone]
        public string Telefone { get; set; } = string.Empty;

        // Chaves estrangeiras
        public Guid EnderecoId { get; set; }
    }

    public class EmpresaUpdateDTO
    {
        public string? Nome { get; set; } = string.Empty;
        public string? CNPJ { get; set; } = string.Empty;
        [EmailAddress]
        public string? Email { get; set; } = string.Empty;
        [Phone]
        public string? Telefone { get; set; }
        // Chaves estrangeiras
        public Guid? EnderecoId { get; set; }
    }

    public class EmpresaResponseDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string? Telefone { get; set; }
        public bool Ativo { get; set; }
        // Chaves estrangeiras
        public EnderecoResponseDTO? Endereco { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime ModificadoEm { get; set; }
    }
}
