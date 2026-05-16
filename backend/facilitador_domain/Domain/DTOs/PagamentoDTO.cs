using System.ComponentModel.DataAnnotations;

namespace facilitador_domain.Domain.DTOs
{
    public class PagamentoCreateDTO
    {
        [Required(ErrorMessage = "O campo 'Cliente' é obrigatório.")]
        public Guid ClienteId { get; set; }

        [Required(ErrorMessage = "O campo 'Empresa' é obrigatório.")]
        public Guid EmpresaId { get; set; }

        [Required(ErrorMessage = "O campo 'Valor do pagamento' é obrigatório.")]
        public decimal ValorPagamento { get; set; }

        public string Observacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Data do pagamento' é obrigatório.")]
        public DateTime DataPagamento { get; set; } = DateTime.UtcNow;
    }

    public class PagamentoUpdateDTO
    {
        public Guid? ClienteId { get; set; }
        public Guid? EmpresaId { get; set; }
        public decimal? ValorPagamento { get; set; }
        public string? Observacao { get; set; }
        public DateTime? DataPagamento { get; set; }
    }

    public class PagamentoResponseDTO
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid EmpresaId { get; set; }
        public decimal ValorPagamento { get; set; }
        public string? Observacao { get; set; }
        public DateTime DataPagamento { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime ModificadoEm { get; set; }
    }
}