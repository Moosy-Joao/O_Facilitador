using facilitador_api.Domain.Entities;
using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Mapping
{
    public static class ClienteMapping
    {
        public static ClienteResponseDTO ToResponseDTO(this Cliente cliente)
        {
            if (cliente == null) return null!;

            return new ClienteResponseDTO
            {
                Id = cliente.Id,
                EmpresaId = cliente.EmpresaId,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Documento = cliente.Documento,
                Telefone = cliente.Telefone,
                Saldo = cliente.Saldo,
                LimiteCredito = cliente.LimiteCredito,
                Inadimplente = cliente.Inadimplente,
                Endereco = cliente.Endereco?.ToResponseDTO(),
                Empresa = cliente.Empresa?.ToResponseDTO(),
                Ativo = cliente.Ativo,
                CriadoEm = cliente.CriadoEm,
                ModificadoEm = cliente.ModificadoEm
            };
        }
    }
}
