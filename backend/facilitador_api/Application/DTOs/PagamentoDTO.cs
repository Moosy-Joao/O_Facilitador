using System.ComponentModel.DataAnnotations;

namespace facilitador_api.Application.DTOs
{
    public class PagamentoDTO
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal PagamentoValor { get; set; }

        [Required(ErrorMessage = "O campo 'Observação' é obrigatório.")]
        public string Observacao { get; set; }

        
    }

    public class PagamentoResponseDTO
    {
        public Guid Id { get; set; }
        public decimal PagamentoValor { get; set; }
        public string? Observacao { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
