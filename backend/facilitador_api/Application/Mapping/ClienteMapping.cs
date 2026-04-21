using facilitador_api.Application.DTOs;
using facilitador_api.Domain.Entities;

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
                Nome = cliente.Nome,
                Email = cliente.Email,
                Documento = cliente.Documento,
                Telefone = cliente.Telefone,
                Saldo = cliente.Saldo,
                LimiteCredito = cliente.LimiteCredito,
                Ativo = cliente.Ativo,
                CriadoEm = cliente.CriadoEm,
                ModificadoEm = cliente.ModificadoEm,
                //Endereco = cliente.Endereco?.ToResponseDTO(),
                //Empresa = cliente.Empresa?.ToResponseDTO()
            };
        }
    }
}
