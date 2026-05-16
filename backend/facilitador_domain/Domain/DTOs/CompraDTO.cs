using System.ComponentModel.DataAnnotations;

namespace facilitador_domain.Domain.DTOs
{
    public class CompraCreateDTO
    {
        [Required(ErrorMessage = "O campo 'Valor' é obrigatório.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo 'Descrição' é obrigatório.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Número da nota' é obrigatório.")]
        public string NumeroNota { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Cliente' é obrigatório.")]
        public Guid ClienteId { get; set; }

        [Required(ErrorMessage = "O campo 'Empresa' é obrigatório.")]
        public Guid EmpresaId { get; set; }
    }

    public class CompraUpdateDTO
    {
        public decimal? Valor { get; set; }
        public string? Descricao { get; set; }
        public string? NumeroNota { get; set; }
        public Guid? ClienteId { get; set; }
        public Guid? EmpresaId { get; set; }
    }

    public class CompraResponseDTO
    {
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string NumeroNota { get; set; } = string.Empty;
        public Guid ClienteId { get; set; }
        public Guid EmpresaId { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime ModificadoEm { get; set; }
    }
}