using facilitador_api.Application.Interfaces;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.API.Controllers
{
    [ApiController]
    [Route("api/v1/compras")]
    public class CompraController : ControllerBase
    {
        private readonly ICompraService _service;

        public CompraController(ICompraService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterCompras()
        {
            var resultado = await _service.BuscarCompras();
            return Ok(resultado);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterCompraPorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);

            if (resultado == null)
            {
                return NotFound("Compra não encontrada.");
            }

            return Ok(resultado);
        }

        [HttpGet("cliente/{clienteId:guid}")]
        public async Task<IActionResult> ObterComprasPorCliente(Guid clienteId)
        {
            var resultado = await _service.BuscarPorCliente(clienteId);
            return Ok(resultado);
        }

        [HttpGet("empresa/{empresaId:guid}")]
        public async Task<IActionResult> ObterComprasPorEmpresa(Guid empresaId)
        {
            var resultado = await _service.BuscarPorEmpresa(empresaId);
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CriarCompra([FromBody] CompraCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);

            if (!resultado)
            {
                return BadRequest("Erro ao criar compra.");
            }

            return Ok("Compra criada com sucesso.");
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> AtualizarCompra(Guid id, [FromBody] CompraUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);

            if (!resultado)
            {
                return BadRequest("Erro ao atualizar compra.");
            }

            return Ok("Compra atualizada com sucesso.");
        }

        [HttpPatch("{id:guid}/ativar")]
        public async Task<IActionResult> AtivarCompra(Guid id)
        {
            var resultado = await _service.Ativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao ativar compra.");
            }

            return Ok("Compra ativada com sucesso.");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DesativarCompra(Guid id)
        {
            var resultado = await _service.Desativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao desativar compra.");
            }

            return Ok("Compra desativada com sucesso.");
        }
    }
}