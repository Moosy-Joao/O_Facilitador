using facilitador_api.Application.DTOs;
using facilitador_api.Domain.Entities;

namespace facilitador_api.Application.Mapping
{
    public static class EnderecoMapping
    {
        public static EnderecoResponseDTO ToResponseDTO(this Endereco endereco) => new()
        {
            Id = endereco.Id,
            Pais = endereco.Pais,
            Estado = endereco.Estado,
            Cidade = endereco.Cidade,
            Bairro = endereco.Bairro,
            Rua = endereco.Rua,
            Numero = endereco.Numero,
            CEP = endereco.CEP,
            Ativo = endereco.Ativo,
            CriadoEm = endereco.CriadoEm,
            ModificadoEm = endereco.ModificadoEm
        };
    }
}
