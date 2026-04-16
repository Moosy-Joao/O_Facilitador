using facilitador_api.Application.DTOs;
using facilitador_api.Application.Interfaces;
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

        public string Atualizar(Guid id, EmpresaDTO dto)
        {
            try
            {
                var endereco = _enderecoRepository.BuscarPorId(dto.Endereco);
                if (endereco == null)
                {
                    return "Endereço não encontrado.";
                }

                var empresa = _empresaRepository.BuscarPorId(id);
                if (empresa == null)
                {
                    return "Empresa não encontrada.";
                }

                var empresaAtualizada = new Empresa(
                    nome: dto.Nome,
                    cnpj: dto.CNPJ,
                    email: dto.Email,
                    telefone: empresa.Telefone,
                    enderecoId: dto.Endereco
                );

                _empresaRepository.Atualizar(empresaAtualizada);
                return "Empresa atualizada com sucesso.";
            }
            catch
            {
                return "Erro ao atualizar empresa.";
            }
        }

        public EmpresaDTO? BuscarPorCNPJ(string cnpj)
        {
            throw new NotImplementedException();
        }

        public EmpresaDTO? BuscarPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmpresaDTO> BuscarPorNome(string nome)
        {
            throw new NotImplementedException();
        }

        public string Criar(EmpresaDTO dto)
        {
            try
            {
                var endereco = _enderecoRepository.BuscarPorId(dto.Endereco);
                if (endereco == null)
                {
                    return "Endereço não encontrado.";
                }

                var empresa = new Empresa(
                    nome: dto.Nome,
                    cnpj: dto.CNPJ,
                    email: dto.Email,
                    telefone: dto.Telefone,
                    enderecoId: dto.Endereco
                );

                _empresaRepository.Cadastrar(empresa);
                return "Empresa criada com sucesso.";
            }
            catch
            {
                return "Erro ao criar empresa.";
            }
        }

        public string Desativar(Guid id)
        {
            try
            {
                var empresa = _empresaRepository.BuscarPorId(id);
                if (empresa == null)
                {
                    return "Empresa não encontrada.";
                }

                _empresaRepository.Desativar(id);
                return "Empresa desativada com sucesso.";
            }
            catch
            {
                return "Erro ao desativar empresa.";
            }
        }
    }
}
