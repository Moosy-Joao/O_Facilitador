using System.ComponentModel.DataAnnotations;

namespace facilitador_api.Application.DTOs
{
    public class ClienteDTO
    {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo 'Nome' deve conter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Documento' é obrigatório.")]
        public string Documento { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Endereco' é obrigatório.")]
        public decimal Saldo { get; set; } = 0;

        [Required(ErrorMessage = "O campo 'LimiteCredito' é obrigatório.")]
        public decimal LimiteCredito { get; set; } = 0;

        // Chaves estrangeiras
        public Guid Endereco { get; set; }
        public Guid Empresa { get; set; }
    }

    public class ClienteResponseDTO : ClienteDTO
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
        public EnderecoDTO? Endereco { get; set; }
        public EmpresaDTO? Empresa { get; set; }
    }
}