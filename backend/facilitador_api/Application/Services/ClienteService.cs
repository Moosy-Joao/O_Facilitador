using facilitador_api.Application.DTOs;
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
            // 1. Buscar o cliente existente (já com tracking do EF Core)
            var cliente = await _clienteRepository.BuscarPorId(id);
            if (cliente == null)
                return false;

            // 2. Atualizar campos simples (apenas se fornecidos no DTO)
            if (!string.IsNullOrWhiteSpace(dto.Nome))
                cliente.AtualizarNome(dto.Nome);

            if (!string.IsNullOrWhiteSpace(dto.Email))
                cliente.AtualizarEmail(dto.Email);

            if (!string.IsNullOrWhiteSpace(dto.Documento))
                cliente.AtualizarDocumento(dto.Documento);

            if (!string.IsNullOrWhiteSpace(dto.Telefone))
                cliente.AtualizarTelefone(dto.Telefone);
            if (dto.Saldo.HasValue)
                cliente.AtualizarSaldo(dto.Saldo.Value);

            if (dto.LimiteCredito.HasValue)
                cliente.AtualizarLimiteCredito(dto.LimiteCredito.Value);

            // 3. Atualizar chave estrangeira EnderecoId (se fornecida e válida)
            if (dto.EnderecoId.HasValue)
            {
                var endereco = await _enderecoRepository.BuscarPorId(dto.EnderecoId.Value);
                if (endereco == null)
                    return false; // Endereço não encontrado
                cliente.AtualizarEnderecoId(dto.EnderecoId.Value);
            }

            // 4. Atualizar chave estrangeira EmpresaId (se fornecida e válida)
            if (dto.EmpresaId.HasValue)
            {
                var empresa = await _empresaRepository.BuscarPorId(dto.EmpresaId.Value);
                if (empresa == null)
                    return false; // Empresa não encontrada
                cliente.AtualizarEmpresaId(dto.EmpresaId.Value);
            }

            // 5. Atualizar timestamp de modificação
            //cliente.AtualizarModificadoEm(DateTime.UtcNow);

            // 6. Persistir as alterações no banco
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

        public async Task<IEnumerable<ClienteResponseDTO>> BuscarPorNome(string nome)
        {
            var clientes = await _clienteRepository.BuscarPorNome(nome);
            if (!clientes.Any())
            {
                return new List<ClienteResponseDTO>();
            }

            return clientes.Select(c => c.ToResponseDTO()).ToList();
        }

        public async Task<bool> Criar(ClienteCreateDTO dto)
        {
            // Verificar se a empresa existe
            var empresa = await _empresaRepository.BuscarPorId(dto.EmpresaId);
            if (empresa == null)
            {
                return false;
            }

            // Verificar se o endereço existe
            var endereco = await _enderecoRepository.BuscarPorId(dto.EnderecoId);
            if (endereco == null)
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
