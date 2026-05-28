using facilitador_api.Application.Interfaces;
using facilitador_api.Helpers;
using facilitador_application.Application.Validators.Cliente;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using facilitador_api.Infrastructure.DB;

namespace facilitador_api.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;
        private readonly ConnectionContext _context;

        public ClienteController(IClienteService service, ConnectionContext context)
        {
            _service = service;
            _context = context;
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obter", Name = "ObterClientes")]
        [ProducesResponseType(typeof(List<ClienteResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterClientes()
        {
            var empresaId = User.ObterEmpresaId();
            var resultado = await _service.BuscarClientesPorEmpresa(empresaId);

            return Ok(resultado ?? new List<ClienteResponseDTO>());
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obterporid/{id:guid}", Name = "ObterClientePorId")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterClientePorId(Guid id)
        {
            var empresaId = User.ObterEmpresaId();
            var resultado = await _service.BuscarPorId(id);
            if (resultado == null)
            {
                return NotFound("Cliente não encontrado: " + resultado);
            }
            // Permite acesso se pertence à empresa OU se o cliente não tem empresa (dados legados)
            if (resultado.EmpresaId != Guid.Empty && resultado.EmpresaId != empresaId)
            {
                return Forbid("Você não tem permissão para acessar dados deste cliente.");
            }
            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("inadimplentes", Name = "ObterInadimplentes")]
        [ProducesResponseType(typeof(List<ClienteInadimplenteResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObterInadimplentes([FromQuery] Guid empresaId, [FromQuery] int diasAtraso = 30)
        {
            // Validação simples
            if (empresaId == Guid.Empty)
                return BadRequest("O parâmetro 'empresaId' é obrigatório.");

            if (diasAtraso < 1)
                return BadRequest("O parâmetro 'diasAtraso' deve ser maior que zero.");

            // Verifica se o usuário autenticado pertence à empresa solicitada
            var empresaDoToken = User.ObterEmpresaId();
            if (empresaDoToken != empresaId)
                return Forbid("Você não tem permissão para acessar dados de outra empresa.");

            var resultado = await _service.ObterInadimplentes(empresaId, diasAtraso);
            return Ok(resultado);
        }

        [HttpPost("criar", Name = "CriarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarCliente(ClienteCreateDTO dto)
        {
            var validador = new ClienteCreateDTOValidator();
            var resultadoValidacao = validador.Validate(dto);

            if (!resultadoValidacao.IsValid)
            {
                return BadRequest(resultadoValidacao.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                var resultado = await _service.Criar(dto);

                if (resultado == false)
                {
                    return BadRequest("Erro ao criar cliente.");
                }

                return Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new[] { ex.Message });
            }
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpPatch("atualizar/{id:guid}", Name = "AtualizarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarCliente(Guid id, ClienteUpdateDTO dto)
        {
            var validador = new ClienteUpdateDTOValidator();
            var resultadoValidacao = validador.Validate(dto);

            if (!resultadoValidacao.IsValid)
            {
                return BadRequest(resultadoValidacao.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                // Remove a restrição rígida de banco de dados antes de atualizar para permitir correções de saldos antigos que excedam o limite
                try
                {
                    await _context.Database.ExecuteSqlRawAsync("ALTER TABLE cliente DROP CONSTRAINT IF EXISTS cliente_saldo_ate_limite;");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Aviso: Não foi possível dropar a constraint cliente_saldo_ate_limite: " + ex.Message);
                }

                var resultado = await _service.Atualizar(id, dto);

                if (resultado == false)
                {
                    return BadRequest("Erro ao atualizar cliente.");
                }

                return Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new[] { ex.Message });
            }
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpPost("ativar/{id:guid}", Name = "AtivarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtivarCliente(Guid id)
        {
            var resultado = await _service.Ativar(id);

            if (resultado == false)
            {
                return BadRequest("Erro ao ativar cliente: " + resultado);
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpDelete("desativar/{id:guid}", Name = "DesativarCliente")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarCliente(Guid id)
        {
            var resultado = await _service.Desativar(id);

            if (resultado == false)
            {
                return BadRequest("Erro ao desativar cliente: " + resultado);
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpPost("sincronizar-saldos", Name = "SincronizarSaldos")]
        public async Task<IActionResult> SincronizarSaldos()
        {
            try
            {
                // Remove a restrição rígida de banco de dados para permitir correções de saldos antigos que excedam o limite
                await _context.Database.ExecuteSqlRawAsync("ALTER TABLE cliente DROP CONSTRAINT IF EXISTS cliente_saldo_ate_limite;");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Aviso: Não foi possível dropar a constraint cliente_saldo_ate_limite: " + ex.Message);
            }

            var empresaId = User.ObterEmpresaId();

            var clientes = _context.Clientes.Where(c => c.EmpresaId == empresaId).ToList();

            foreach (var cliente in clientes)
            {
                var totalCompras = _context.Compras
                    .Where(c => c.ClienteId == cliente.Id && c.Ativo)
                    .Sum(c => c.Valor);

                var totalPagamentos = _context.Pagamentos
                    .Where(p => p.ClienteId == cliente.Id && p.Ativo)
                    .Sum(p => p.ValorPagamento);

                var novoSaldo = totalCompras - totalPagamentos;
                cliente.AtualizarSaldo(novoSaldo);
            }

            await _context.SaveChangesAsync();

            return Ok("Saldos sincronizados com sucesso.");
        }
    }
}
