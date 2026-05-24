using facilitador_api.Application.Interfaces;
using facilitador_api.Application.Mapping;
using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public PagamentoService(
            IPagamentoRepository pagamentoRepository,
            IClienteRepository clienteRepository,
            IEmpresaRepository empresaRepository)
        {
            _pagamentoRepository = pagamentoRepository;
            _clienteRepository = clienteRepository;
            _empresaRepository = empresaRepository;
        }

        public async Task<List<PagamentoResponseDTO>> BuscarPagamentos()
        {
            var pagamentos = await _pagamentoRepository.BuscarTodos();
            return pagamentos.Select(p => p.ToResponseDTO()).ToList();
        }

        public async Task<PagamentoResponseDTO?> BuscarPorId(Guid id)
        {
            var pagamento = await _pagamentoRepository.BuscarPorId(id);
            return pagamento?.ToResponseDTO();
        }

        public async Task<List<PagamentoResponseDTO>> BuscarPorCliente(Guid clienteId)
        {
            var pagamentos = await _pagamentoRepository.BuscarPorCliente(clienteId);
            return pagamentos.Select(p => new PagamentoResponseDTO
            {
                Id = p.Id,
                ValorPagamento = p.ValorPagamento,
                Observacao = p.Observacao,
                Ativo = p.Ativo,
                CriadoEm = p.CriadoEm
            }).ToList();
        }

        public async Task<List<PagamentoResponseDTO>> BuscarPorEmpresa(Guid empresaId)
        {
            var pagamentos = await _pagamentoRepository.BuscarPorEmpresa(empresaId);

            return pagamentos.Select(p => p.ToResponseDTO()).ToList();
        }

        public async Task<List<PagamentoResponseDTO>> BuscarPorData(DateTime dataPagamento)
        {
            var pagamentos = await _pagamentoRepository.BuscarPorData(dataPagamento);
            return pagamentos.Select(p => p.ToResponseDTO()).ToList();
        }

        public async Task<bool> Criar(PagamentoCreateDTO dto, Guid EmpresaId)
        {
            var cliente = await _clienteRepository.BuscarPorId(dto.ClienteId);
            if (cliente == null || !cliente.Ativo)
            {
                throw new Exception("Cliente não encontrado ou inativo.");
            }

            decimal novoSaldo = cliente.Saldo - dto.ValorPagamento;

            if (novoSaldo > cliente.LimiteCredito)
            {
                novoSaldo = cliente.LimiteCredito;
            }

            var pagamento = new Pagamento(
                clienteId: dto.ClienteId,
                empresaId: EmpresaId,
                valorPagamento: dto.ValorPagamento,
                observacao: dto.Observacao,
                dataPagamento: dto.DataPagamento
            );

            var trasacao = await _pagamentoRepository.IniciarTransacao();
            try
            {
                await _pagamentoRepository.Cadastrar(pagamento);
                cliente.AtualizarSaldo(novoSaldo);
                await _pagamentoRepository.Salvar();
                await _clienteRepository.Atualizar(cliente);
                await _clienteRepository.Salvar();
                await trasacao.CommitAsync();
                return true;
            }
            catch
            {
                await trasacao.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Atualizar(Guid id, PagamentoUpdateDTO dto)
        {
            var pagamento = await _pagamentoRepository.BuscarPorId(id);
            if (pagamento == null)
            {
                return false;
            }

            if (dto.ValorPagamento.HasValue)
            {
                if (dto.ValorPagamento.Value <= 0)
                {
                    return false;
                }

                pagamento.AtualizarValorPagamento(dto.ValorPagamento.Value);
            }

            if (!string.IsNullOrWhiteSpace(dto.Observacao))
            {
                pagamento.AtualizarObservacao(dto.Observacao);
            }

            if (dto.DataPagamento.HasValue)
            {
                pagamento.AtualizarDataPagamento(dto.DataPagamento.Value);
            }

            if (dto.ClienteId.HasValue)
            {
                var clienteExiste = await _clienteRepository.Existe(dto.ClienteId.Value);
                if (!clienteExiste)
                {
                    return false;
                }

                pagamento.AtualizarClienteId(dto.ClienteId.Value);
            }

            if (dto.EmpresaId.HasValue)
            {
                var empresaExiste = await _empresaRepository.Existe(dto.EmpresaId.Value);
                if (!empresaExiste)
                {
                    return false;
                }

                pagamento.AtualizarEmpresaId(dto.EmpresaId.Value);
            }

            pagamento.AtualizarModificadoEm(DateTime.UtcNow);

            await _pagamentoRepository.Atualizar(pagamento);
            await _pagamentoRepository.Salvar();

            return true;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var existe = await _pagamentoRepository.Existe(id);
            if (!existe)
            {
                return false;
            }

            await _pagamentoRepository.Desativar(id);
            await _pagamentoRepository.Salvar();

            return true;
        }

        public async Task<bool> Ativar(Guid id)
        {
            var existe = await _pagamentoRepository.Existe(id);
            if (!existe)
            {
                return false;
            }

            await _pagamentoRepository.Ativar(id);
            await _pagamentoRepository.Salvar();

            return true;
        }
    }
}