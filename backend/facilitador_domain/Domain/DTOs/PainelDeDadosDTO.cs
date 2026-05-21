namespace facilitador_domain.Domain.DTOs
{
    public class PainelDeDadosDTO
    {
        public decimal TotalReceber { get; set; }
        public int Inadimplentes { get; set; }
        public decimal InadimplentesValor { get; set; }
        public int TotalClientes { get; set; }
        public int NovosClientesSemana { get; set; }
        public decimal VendasHoje { get; set; }
        public decimal PagamentosHoje { get; set; }
    }

    public class PainelDeTransacoesDTO
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Hora { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class PainelDeGraficosDTO
    {
        public string Data { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
