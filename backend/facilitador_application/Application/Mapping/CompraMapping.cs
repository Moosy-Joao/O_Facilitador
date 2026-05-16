using facilitador_api.Domain.Entities;
using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Mapping
{
    public static class CompraMapping
    {
        public static CompraResponseDTO ToResponseDTO(this Compra compra)
        {
            return new CompraResponseDTO
            {
                Id = compra.Id,
                Valor = compra.Valor,
                Descricao = compra.Descricao,
                NumeroNota = compra.NumeroNota,
                ClienteId = compra.ClienteId,
                EmpresaId = compra.EmpresaId,
                Ativo = compra.Ativo,
                CriadoEm = compra.CriadoEm,
                ModificadoEm = compra.ModificadoEm
            };
        }
    }
}