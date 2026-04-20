using facilitador_api.Application.DTOs;
using facilitador_api.Application.Interfaces;
using facilitador_api.Application.Mapping;
using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;

namespace facilitador_api.Application.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public EmpresaService(IEmpresaRepository empresaRepository, IEnderecoRepository enderecoRepository)
        {
            _empresaRepository = empresaRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task<bool> Atualizar(Guid id, EmpresaUpdateDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<EmpresaResponseDTO?> BuscarPorCNPJ(string cnpj)
        {
            var empresa = await _empresaRepository.BuscarPorCNPJ(cnpj);
            return empresa?.ToResponseDTO();
        }

        public async Task<EmpresaResponseDTO?> BuscarPorId(Guid id)
        {
            var empresa = await _empresaRepository.BuscarPorId(id);
            return empresa?.ToResponseDTO();
        }

        public async Task<IEnumerable<EmpresaResponseDTO>?> BuscarPorNome(string nome)
        {
            var empresas = await _empresaRepository.BuscarPorNome(nome);
            if (!empresas.Any())
            {
                return null;
            }

            return empresas.Select(e => e.ToResponseDTO()).ToList();
        }

        public async Task<bool> Criar(EmpresaCreateDTO dto)
        {
            var endereco = await _enderecoRepository.BuscarPorId(dto.EnderecoId);
            if (endereco == null)
            {
                return false;
            }

            var empresa = new Empresa(dto, dto.EnderecoId);

            await _empresaRepository.Cadastrar(empresa);
            await _empresaRepository.Salvar();

            return true;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var empresa = await _empresaRepository.BuscarPorId(id);
            if (empresa == null)
            {
                return false;
            }

            await _empresaRepository.Desativar(id);

            await _empresaRepository.Salvar();
            return true;
        }
    }
}
