using facilitador_api.Domain.Entities;
using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Mapping
{
    public static class PagamentoMapping
    {
        public static PagamentoResponseDTO ToResponseDTO(this Pagamento pagamento)
        {
            return new PagamentoResponseDTO
            {
                Id = pagamento.Id,
                ClienteId = pagamento.ClienteId,
                EmpresaId = pagamento.EmpresaId,
                ValorPagamento = pagamento.ValorPagamento,
                Observacao = pagamento.Observacao,
                DataPagamento = pagamento.DataPagamento,
                Ativo = pagamento.Ativo,
                CriadoEm = pagamento.CriadoEm,
                ModificadoEm = pagamento.ModificadoEm
            };
        }
    }
}