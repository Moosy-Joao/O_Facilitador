using facilitador_api.Application.DTOs;
using facilitador_api.Application.Interfaces;
using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;

namespace facilitador_api.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public ClienteService(IClienteRepository repository, IEmpresaRepository empresaRepository, IEnderecoRepository enderecoRepository)
        {
            _clienteRepository = repository;
            _empresaRepository = empresaRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task<bool> Atualizar(Guid id, ClienteUpdateDTO dto)
        {
            // Verificar se a empresa existe
            //var empresa = _empresaRepository.BuscarPorId(dto.Empresa);
            //if (empresa == null)
            //{
            //    return false;
            //}

            // Verificar se o endereço existe
            //var endereco = _enderecoRepository.BuscarPorId(dto.Endereco);
            //if (endereco == null)
            //{
            //    return false;
            //}

            //var cliente = _clienteRepository.BuscarPorId(id);

            //if (cliente == null)
            //{
            //    return false;
            //}

            //var clienteAtualizado = new ClienteUpdateDTO();

            //_clienteRepository.Atualizar(clienteAtualizado);
            //return true;

            throw new NotImplementedException();
        }

        public async Task<ClienteResponseDTO?> BuscarPorDocumento(string documento)
        {
            var cliente = _clienteRepository.BuscarPorDocumento(documento);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado.");
            }

            var endereco = _enderecoRepository.BuscarPorId(cliente.Id);
            if (endereco == null)
            {
                throw new Exception("Endereço do cliente não encontrado.");
            }

            var empresa = _empresaRepository.BuscarPorId(cliente.Id);
            if (empresa == null)
            {
                throw new Exception("Empresa do cliente não encontrada.");
            }

            return new ClienteResponseDTO();
        }

        public async Task<ClienteResponseDTO?> BuscarPorEmail(string email)
        {
            var cliente = _clienteRepository.BuscarPorEmail(email);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado.");
            }

            var endereco = _enderecoRepository.BuscarPorId(cliente.Id);
            if (endereco == null)
            {
                throw new Exception("Endereço do cliente não encontrado.");
            }

            var empresa = _empresaRepository.BuscarPorId(cliente.Id);
            if (empresa == null)
            {
                throw new Exception("Empresa do cliente não encontrada.");
            }

            return new ClienteResponseDTO();
        }

        public async Task<ClienteResponseDTO?> BuscarPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ClienteResponseDTO>> BuscarPorNome(string nome)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Criar(ClienteCreateDTO dto)
        {
            // Verificar se a empresa existe
            var empresa = _empresaRepository.BuscarPorId(dto.Empresa);
            if (empresa == null)
            {
                return false;
            }

            // Verificar se o endereço existe
            var endereco = _enderecoRepository.BuscarPorId(dto.Endereco);
            if (endereco == null)
            {
                return false;
            }

            var clienteNovo = new Cliente(
                dto,
                dto.Empresa,
                dto.Endereco
            );

            await _clienteRepository.Cadastrar(clienteNovo);
            return true;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var cliente = await _clienteRepository.BuscarPorId(id);
            if (cliente == null)
            {
                return false;
            }

            cliente.Desativar();

            await _clienteRepository.Salvar();
            return true;
        }
    }
}
