using System.ComponentModel.DataAnnotations;

namespace facilitador_domain.Domain.DTOs
{
    public class EnderecoDTO
    {
        [Required(ErrorMessage = "O campo 'Pais' é obrigatório.")]
        public string Pais { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Estado' é obrigatório.")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Cidade' é obrigatório.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Bairro' é obrigatório.")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Rua' é obrigatório.")]
        public string Rua { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Numero' é obrigatório.")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'CEP' é obrigatório.")]
        public string CEP { get; set; } = string.Empty;
    }

    public class EnderecoCreateDTO : EnderecoDTO
    {
    }

    public class EnderecoUpdateDTO
    {
        public string? Pais { get; set; } = string.Empty;
        public string? Estado { get; set; } = string.Empty;
        public string? Cidade { get; set; } = string.Empty;
        public string? Bairro { get; set; } = string.Empty;
        public string? Rua { get; set; } = string.Empty;
        public string? Numero { get; set; } = string.Empty;
        public string? CEP { get; set; } = string.Empty;
    }

    public class EnderecoResponseDTO
    {
        public Guid Id { get; set; }
        public string Pais { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? ModificadoEm { get; set; }
    }
}
