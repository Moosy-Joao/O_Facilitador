namespace facilitador_api.Domain.Entities
{
    public class Pagamento : BaseModel
    {
        public decimal ValorPagamento { get; private set; }
        public string? Observacao { get; private set; }
        public DateTime DataPagamento { get; private set; }

        // Relacionamento com Cliente
        public Guid ClienteId { get; private set; }
        public Cliente? Cliente { get; private set; }

        // Relacionamento com Empresa
        public Guid EmpresaId { get; private set; }
        public Empresa? Empresa { get; private set; }

        public Pagamento() { }

        public Pagamento(Guid clienteId, Guid empresaId, decimal valorPagamento, string observacao, DateTime dataPagamento)
        {
            ClienteId = clienteId;
            EmpresaId = empresaId;
            ValorPagamento = valorPagamento;
            Observacao = observacao;
            DataPagamento = dataPagamento;
        }

        //public Pagamento(PagamentoCreateDTO dto, Guid clienteId, Guid empresaId)
        //{
        //    ClienteId = clienteId;
        //    EmpresaId = empresaId;
        //    ValorPagamento = dto.ValorPagamento;
        //    Observacao = dto.Observacao;
        //    DataPagamento = dto.DataPagamento;
        //}

        public void AtualizarValorPagamento(decimal novoValor)
        {
            if (novoValor <= 0) return;
            ValorPagamento = novoValor;
        }

        public void AtualizarObservacao(string novaObservacao)
        {
            if (string.IsNullOrEmpty(novaObservacao)) return;
            Observacao = novaObservacao;
        }

        public void AtualizarDataPagamento(DateTime novaData)
        {
            DataPagamento = novaData;
        }

        public void AtualizarClienteId(Guid clienteId) => ClienteId = clienteId;

        public void AtualizarEmpresaId(Guid empresaId) => EmpresaId = empresaId;
    }
}
