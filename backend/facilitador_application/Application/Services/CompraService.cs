using facilitador_api.Application.Interfaces;
using facilitador_api.Application.Mapping;
using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public CompraService(
            ICompraRepository compraRepository,
            IClienteRepository clienteRepository,
            IEmpresaRepository empresaRepository)
        {
            _compraRepository = compraRepository;
            _clienteRepository = clienteRepository;
            _empresaRepository = empresaRepository;
        }

        public async Task<List<CompraResponseDTO>> BuscarCompras()
        {
            var compras = await _compraRepository.BuscarTodos();
            return compras.Select(c => c.ToResponseDTO()).ToList();
        }

        public async Task<CompraResponseDTO?> BuscarPorId(Guid id)
        {
            var compra = await _compraRepository.BuscarPorId(id);
            return compra?.ToResponseDTO();
        }

        public async Task<List<CompraResponseDTO>> BuscarPorCliente(Guid clienteId)
        {
            var compras = await _compraRepository.BuscarPorCliente(clienteId);
            return compras.Select(c => c.ToResponseDTO()).ToList();
        }

        public async Task<List<CompraResponseDTO>> BuscarPorEmpresa(Guid empresaId)
        {
            var compras = await _compraRepository.BuscarPorEmpresa(empresaId);
            return compras.Select(c => c.ToResponseDTO()).ToList();
        }

        public async Task<bool> Criar(CompraCreateDTO dto)
        {
            var clienteExiste = await _clienteRepository.Existe(dto.ClienteId);
            if (!clienteExiste)
            {
                return false;
            }

            var empresaExiste = await _empresaRepository.Existe(dto.EmpresaId);
            if (!empresaExiste)
            {
                return false;
            }

            if (dto.Valor <= 0)
            {
                return false;
            }

            var compra = new Compra(
                dto.Valor,
                dto.Descricao,
                dto.NumeroNota,
                dto.ClienteId,
                dto.EmpresaId
            );

            await _compraRepository.Cadastrar(compra);
            await _compraRepository.Salvar();

            return true;
        }

        public async Task<bool> Atualizar(Guid id, CompraUpdateDTO dto)
        {
            var compra = await _compraRepository.BuscarPorId(id);
            if (compra == null)
            {
                return false;
            }

            if (dto.Valor.HasValue)
            {
                if (dto.Valor.Value <= 0)
                {
                    return false;
                }

                compra.AtualizarValor(dto.Valor.Value);
            }

            if (!string.IsNullOrWhiteSpace(dto.Descricao))
            {
                compra.AtualizarDescricao(dto.Descricao);
            }

            if (!string.IsNullOrWhiteSpace(dto.NumeroNota))
            {
                compra.AtualizarNumeroNota(dto.NumeroNota);
            }

            if (dto.ClienteId.HasValue)
            {
                var clienteExiste = await _clienteRepository.Existe(dto.ClienteId.Value);
                if (!clienteExiste)
                {
                    return false;
                }

                compra.AtualizarCliente(dto.ClienteId.Value);
            }

            if (dto.EmpresaId.HasValue)
            {
                var empresaExiste = await _empresaRepository.Existe(dto.EmpresaId.Value);
                if (!empresaExiste)
                {
                    return false;
                }

                compra.AtualizarEmpresa(dto.EmpresaId.Value);
            }

            compra.AtualizarModificadoEm(DateTime.UtcNow);

            await _compraRepository.Atualizar(compra);
            await _compraRepository.Salvar();

            return true;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var existe = await _compraRepository.Existe(id);
            if (!existe)
            {
                return false;
            }

            await _compraRepository.Desativar(id);
            await _compraRepository.Salvar();

            return true;
        }

        public async Task<bool> Ativar(Guid id)
        {
            var existe = await _compraRepository.Existe(id);
            if (!existe)
            {
                return false;
            }

            await _compraRepository.Ativar(id);
            await _compraRepository.Salvar();

            return true;
        }
    }
}