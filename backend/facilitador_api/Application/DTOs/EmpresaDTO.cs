using System.ComponentModel.DataAnnotations;

namespace facilitador_api.Application.DTOs
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
        public string Telefone { get; set; }

        // Chaves estrangeiras
        public Guid Endereco { get; set; }
    }

    public class EmpresaResponseDTO : EmpresaDTO
    {
        public Guid Id { get; set; }
        public EnderecoDTO? Endereco { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
