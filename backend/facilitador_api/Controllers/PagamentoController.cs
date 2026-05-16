using facilitador_api.Application.Interfaces;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.API.Controllers
{
    [ApiController]
    [Route("api/v1/pagamentos")]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _service;

        public PagamentoController(IPagamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterPagamentos()
        {
            var resultado = await _service.BuscarPagamentos();
            return Ok(resultado);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPagamentoPorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);

            if (resultado == null)
            {
                return NotFound("Pagamento não encontrado.");
            }

            return Ok(resultado);
        }

        [HttpGet("cliente/{clienteId:guid}")]
        public async Task<IActionResult> ObterPagamentosPorCliente(Guid clienteId)
        {
            var resultado = await _service.BuscarPorCliente(clienteId);
            return Ok(resultado);
        }

        [HttpGet("empresa/{empresaId:guid}")]
        public async Task<IActionResult> ObterPagamentosPorEmpresa(Guid empresaId)
        {
            var resultado = await _service.BuscarPorEmpresa(empresaId);
            return Ok(resultado);
        }

        [HttpGet("data")]
        public async Task<IActionResult> ObterPagamentosPorData([FromQuery] DateTime dataPagamento)
        {
            var resultado = await _service.BuscarPorData(dataPagamento);
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CriarPagamento([FromBody] PagamentoCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);

            if (!resultado)
            {
                return BadRequest("Erro ao criar pagamento.");
            }

            return Ok("Pagamento criado com sucesso.");
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> AtualizarPagamento(Guid id, [FromBody] PagamentoUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);

            if (!resultado)
            {
                return BadRequest("Erro ao atualizar pagamento.");
            }

            return Ok("Pagamento atualizado com sucesso.");
        }

        [HttpPatch("{id:guid}/ativar")]
        public async Task<IActionResult> AtivarPagamento(Guid id)
        {
            var resultado = await _service.Ativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao ativar pagamento.");
            }

            return Ok("Pagamento ativado com sucesso.");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DesativarPagamento(Guid id)
        {
            var resultado = await _service.Desativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao desativar pagamento.");
            }

            return Ok("Pagamento desativado com sucesso.");
        }
    }
}
