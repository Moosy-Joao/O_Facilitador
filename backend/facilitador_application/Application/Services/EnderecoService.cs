using facilitador_api.Application.DTOs;
using facilitador_api.Application.Interfaces;
using facilitador_api.Application.Mapping;
using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;

namespace facilitador_api.Application.Services
{
    public class EnderecoService : IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;

        public EnderecoService(IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        public async Task<bool> Atualizar(Guid id, EnderecoUpdateDTO dto)
        {
            var endereco = await _enderecoRepository.BuscarPorId(id);
            if (endereco == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(dto.Pais))
            {
                endereco.AtualizarPais(dto.Pais);
            }
            if (!string.IsNullOrEmpty(dto.Estado))
            {
                endereco.AtualizarEstado(dto.Estado);
            }
            if (!string.IsNullOrEmpty(dto.Cidade))
            {
                endereco.AtualizarCidade(dto.Cidade);
            }
            if (!string.IsNullOrEmpty(dto.Bairro))
            {
                endereco.AtualizarBairro(dto.Bairro);
            }
            if (!string.IsNullOrEmpty(dto.Rua))
            {
                endereco.AtualizarRua(dto.Rua);
            }
            if (!string.IsNullOrEmpty(dto.Numero))
            {
                endereco.AtualizarNumero(dto.Numero);
            }
            if (!string.IsNullOrEmpty(dto.CEP))
            {
                endereco.AtualizarCEP(dto.CEP);
            }

            await _enderecoRepository.Salvar();

            return true;
        }

        public async Task<EnderecoResponseDTO?> BuscarPorCEP(string cep)
        {
            var endereco = await _enderecoRepository.BuscarPorCEP(cep);
            return endereco?.ToResponseDTO();
        }

        public async Task<EnderecoResponseDTO?> BuscarPorId(Guid id)
        {
            var endereco = await _enderecoRepository.BuscarPorId(id);
            return endereco?.ToResponseDTO();
        }

        public async Task<List<EnderecoResponseDTO>?> BuscarEnderecos()
        {
            var enderecos = await _enderecoRepository.BuscarTodos();
            return enderecos.Select(e => e.ToResponseDTO()).ToList();
        }

        public async Task<bool> Criar(EnderecoCreateDTO dto)
        {
            var enderecoNovo = new Endereco(dto);

            await _enderecoRepository.Cadastrar(enderecoNovo);
            await _enderecoRepository.Salvar();

            return true;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var endereco = await _enderecoRepository.BuscarPorId(id);
            if (endereco == null)
            {
                return false;
            }

            endereco.Desativar();
            await _enderecoRepository.Salvar();

            return true;
        }
    }
}
