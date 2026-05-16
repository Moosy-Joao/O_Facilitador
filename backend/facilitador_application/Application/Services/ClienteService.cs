using facilitador_domain.Domain.DTOs;
using facilitador_api.Application.Interfaces;
using facilitador_api.Application.Mapping;
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
            // 1. Buscar o cliente existente
            var cliente = await _clienteRepository.BuscarPorId(id);
            if (cliente == null)
            {
                return false;
            }

            // 2. Atualizar campos simples
            if (!string.IsNullOrWhiteSpace(dto.Nome))
            { cliente.AtualizarNome(dto.Nome); }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            { cliente.AtualizarEmail(dto.Email); }

            if (!string.IsNullOrWhiteSpace(dto.Documento))
            { cliente.AtualizarDocumento(dto.Documento); }

            if (!string.IsNullOrWhiteSpace(dto.Telefone))
            { cliente.AtualizarTelefone(dto.Telefone); }
            if (dto.Saldo.HasValue)
            { cliente.AtualizarSaldo(dto.Saldo.Value); }

            if (dto.LimiteCredito.HasValue)
            { cliente.AtualizarLimiteCredito(dto.LimiteCredito.Value); }

            // 3. Atualizar chave estrangeira EnderecoId
            if (dto.EnderecoId.HasValue)
            {
                var endereco = await _enderecoRepository.BuscarPorId(dto.EnderecoId.Value);
                if (endereco == null)
                {
                    return false;
                }
                cliente.AtualizarEnderecoId(dto.EnderecoId.Value);
            }

            // 4. Atualizar chave estrangeira EmpresaId
            if (dto.EmpresaId.HasValue)
            {
                var empresa = await _empresaRepository.BuscarPorId(dto.EmpresaId.Value);
                if (empresa == null)
                {
                    return false;
                }
                cliente.AtualizarEmpresaId(dto.EmpresaId.Value);
            }

            // 5. Atualizar timestamp de modificação
            //cliente.AtualizarModificadoEm(DateTime.UtcNow);

            await _clienteRepository.Salvar();

            return true;
        }

        public async Task<ClienteResponseDTO?> BuscarPorDocumento(string documento)
        {
            var cliente = await _clienteRepository.BuscarPorDocumento(documento);
            return cliente?.ToResponseDTO();
        }

        public async Task<ClienteResponseDTO?> BuscarPorEmail(string email)
        {
            var cliente = await _clienteRepository.BuscarPorEmail(email);
            return cliente?.ToResponseDTO();
        }

        public async Task<ClienteResponseDTO?> BuscarPorId(Guid id)
        {
            var cliente = await _clienteRepository.BuscarPorId(id);
            return cliente?.ToResponseDTO();
        }

        public async Task<List<ClienteResponseDTO>> BuscarPorNome(string nome)
        {
            var clientes = await _clienteRepository.BuscarPorNome(nome);
            if (!clientes.Any())
            {
                return new List<ClienteResponseDTO>();
            }

            return clientes.Select(c => c.ToResponseDTO()).ToList();
        }

        public async Task<List<ClienteResponseDTO>> BuscarClientes()
        {
            var clientes = await _clienteRepository.BuscarTodos();
            return clientes.Select(c => c.ToResponseDTO()).ToList();
        }

        public async Task<bool> Criar(ClienteCreateDTO dto)
        {
            var empresaExiste = await _empresaRepository.Existe(dto.EmpresaId);
            if (!empresaExiste)
            {
                return false;
            }

            var enderecoExiste = await _enderecoRepository.Existe(dto.EnderecoId);
            if (!enderecoExiste)
            {
                return false;
            }

            var clienteNovo = new Cliente(dto, dto.EmpresaId, dto.EnderecoId);

            await _clienteRepository.Cadastrar(clienteNovo);
            await _clienteRepository.Salvar();

            return true;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var cliente = await _clienteRepository.Existe(id);
            if (!cliente)
            {
                return false;
            }

            await _clienteRepository.Desativar(id);
            await _clienteRepository.Salvar();

            return true;
        }

        public async Task<bool> Ativar(Guid id)
        {
            var cliente = await _clienteRepository.Existe(id);
            if (!cliente)
            {
                return false;
            }

            await _clienteRepository.Ativar(id);
            await _clienteRepository.Salvar();

            return true;
        }
    }
}
