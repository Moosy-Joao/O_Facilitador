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

        [HttpGet("obter", Name = "ObterCompras")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterCompras()
        {
            var resultado = await _service.BuscarCompras();

            if (resultado == null)
            {
                return NotFound("Nenhuma compra encontrada: " + resultado);
            }

            return Ok(resultado);
        }

        [HttpGet("obterporid/{id:guid}", Name = "ObterCompraPorId")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterCompraPorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);

            if (resultado == null)
            {
                return NotFound("Compra não encontrada: " + resultado);
            }

            return Ok(resultado);
        }

        [HttpGet("obterporcliente/{clienteId:guid}", Name = "ObterComprasPorCliente")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterComprasPorCliente(Guid clienteId)
        {
            var resultado = await _service.BuscarPorCliente(clienteId);

            if (resultado == null)
            {
                return NotFound("Nenhuma compra encontrada para o cliente: " + clienteId);
            }

            return Ok(resultado);
        }

        [HttpGet("obterporempresa/{empresaId:guid}", Name = "ObterComprasPorEmpresa")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterComprasPorEmpresa(Guid empresaId)
        {
            var resultado = await _service.BuscarPorEmpresa(empresaId);

            if (resultado == null)
            {
                return NotFound("Nenhuma compra encontrada para a empresa: " + resultado);
            }

            return Ok(resultado);
        }

        [HttpPost("criar", Name = "CriarCompra")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarCompra([FromBody] CompraCreateDTO dto)
        {
            var resultado = await _service.Criar(dto);

            if (!resultado)
            {
                return BadRequest("Erro ao criar compra: " + resultado);
            }

            return Ok("Compra criada com sucesso.");
        }

        [HttpPatch("atualizar/{id:guid}", Name = "AtualizarCompra")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarCompra(Guid id, [FromBody] CompraUpdateDTO dto)
        {
            var resultado = await _service.Atualizar(id, dto);

            if (!resultado)
            {
                return BadRequest("Erro ao atualizar compra: " + resultado);
            }

            return Ok("Compra atualizada com sucesso.");
        }

        [HttpPatch("ativar/{id:guid}", Name = "AtivarCompra")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtivarCompra(Guid id)
        {
            var resultado = await _service.Ativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao ativar compra: " + resultado);
            }

            return Ok("Compra ativada com sucesso.");
        }

        [HttpDelete("desativar/{id:guid}", Name = "DesativarCompra")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarCompra(Guid id)
        {
            var resultado = await _service.Desativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao desativar compra: " + resultado);
            }

            return Ok("Compra desativada com sucesso.");
        }
    }
}