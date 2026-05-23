using facilitador_api.Application.Interfaces;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using facilitador_api.Helpers;

namespace facilitador_api.API.Controllers
{ 
    [Authorize]
    [ApiController]
    [Route("api/v1/pagamentos")]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _service;

        public PagamentoController(IPagamentoService service)
        {
            _service = service;
        }
        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obter", Name = "ObterPagamentos")]
        [ProducesResponseType(typeof(PagamentoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPagamentos()
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.BuscarPorEmpresa(empresaId);

            if (resultado == null || !resultado.Any())
            {
                return NotFound("Nenhum pagamento encontrado.");
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obterporid/{id:guid}", Name = "ObterPagamentoPorId")]
        [ProducesResponseType(typeof(PagamentoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPagamentoPorId(Guid id)
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.BuscarPorId(id);

            if (resultado == null)
            {
                return NotFound("Pagamento não encontrado.");
            }

            if (resultado.EmpresaId != empresaId)
            {
                return Forbid("Você não tem permissão para acessar este pagamento.");
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obterporcliente/{clienteId:guid}", Name = "ObterPagamentosPorCliente")]
        [ProducesResponseType(typeof(List<PagamentoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPagamentosPorCliente(Guid clienteId)
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.BuscarPorCliente(clienteId);

            if (resultado == null || !resultado.Any())
            {
                return NotFound("Nenhum pagamento encontrado para este cliente.");
            }

            var existePagamentoDeOutraEmpresa = resultado.Any(p => p.EmpresaId != empresaId);

            if (existePagamentoDeOutraEmpresa)
            {
                return Forbid("Você não tem permissão para acessar pagamentos de outra empresa.");
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obterporempresa", Name = "ObterPagamentosPorEmpresa")]
        [ProducesResponseType(typeof(PagamentoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPagamentosPorEmpresa()
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.BuscarPorEmpresa(empresaId);

            if (resultado == null || !resultado.Any())
            {
                return NotFound("Nenhum pagamento encontrado para sua empresa.");
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obterpordata", Name = "ObterPagamentosPorData")]
        [ProducesResponseType(typeof(List<PagamentoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPagamentosPorData([FromQuery] DateTime dataPagamento)
        {
            var resultado = await _service.BuscarPorData(dataPagamento);

            if (resultado == null || !resultado.Any())
            {
                return NotFound("Nenhum pagamento encontrado para a data: " + resultado);
            }

            return Ok(resultado);
        }
        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpPost("criar", Name = "CriarPagamento")]
        [ProducesResponseType(typeof(PagamentoResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarPagamento([FromBody] PagamentoCreateDTO dto)
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.Criar(dto, empresaId);

            if (!resultado)
            {
                return BadRequest("Erro ao criar pagamento: " + resultado);
            }

            return Ok("Pagamento criado com sucesso: " + resultado);
        }
        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpPatch("atualizar/{id:guid}", Name = "AtualizarPagamento")]
        [ProducesResponseType(typeof(PagamentoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarPagamento(Guid id, [FromBody] PagamentoUpdateDTO dto)
        {
            var empresaId = User.ObterEmpresaId();

            var pagamento = await _service.BuscarPorId(id);

            if (pagamento == null)
            {
                return NotFound("Pagamento não encontrado.");
            }

            if (pagamento.EmpresaId != empresaId)
            {
                return Forbid("Você não tem permissão para atualizar este pagamento.");
            }

            var resultado = await _service.Atualizar(id, dto);

            if (!resultado)
            {
                return BadRequest("Erro ao atualizar pagamento.");
            }

            return Ok("Pagamento atualizado com sucesso.");
        }

        [Authorize(Policy = "Gerente")]
        [HttpPatch("ativar/{id:guid}", Name = "AtivarPagamento")]
        [ProducesResponseType(typeof(PagamentoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtivarPagamento(Guid id)
        {
            var empresaId = User.ObterEmpresaId();

            var pagamento = await _service.BuscarPorId(id);

            if (pagamento == null)
            {
                return NotFound("Pagamento não encontrado.");
            }

            if (pagamento.EmpresaId != empresaId)
            {
                return Forbid("Você não tem permissão para ativar este pagamento.");
            }

            var resultado = await _service.Ativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao ativar pagamento.");
            }

            return Ok("Pagamento ativado com sucesso.");
        }

        [Authorize(Policy = "Gerente")]
        [HttpDelete("desativar/{id:guid}", Name = "DesativarPagamento")]
        [ProducesResponseType(typeof(PagamentoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarPagamento(Guid id)
        {
            var empresaId = User.ObterEmpresaId();

            var pagamento = await _service.BuscarPorId(id);

            if (pagamento == null)
            {
                return NotFound("Pagamento não encontrado.");
            }

            if (pagamento.EmpresaId != empresaId)
            {
                return Forbid("Você não tem permissão para desativar este pagamento.");
            }

            var resultado = await _service.Desativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao desativar pagamento.");
            }

            return Ok("Pagamento desativado com sucesso.");
        }
    }
}
