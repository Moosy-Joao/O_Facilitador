using facilitador_api.Application.Interfaces;
using facilitador_api.Helpers;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace facilitador_api.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/compras")]
    public class CompraController : ControllerBase
    {
        private readonly ICompraService _service;

        public CompraController(ICompraService service)
        {
            _service = service;
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obter", Name = "ObterCompras")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterCompras()
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.BuscarPorEmpresa(empresaId);

            return Ok(resultado ?? new List<CompraResponseDTO>());
        }
        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obterporid/{id:guid}", Name = "ObterCompraPorId")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterCompraPorId(Guid id)
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.BuscarPorId(id);

            if (resultado == null)
            {
                return NotFound("Compra não encontrada.");
            }

            if (resultado.EmpresaId != empresaId)
            {
                return Forbid("Você não tem permissão para acessar esta compra.");
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obterporcliente/{clienteId:guid}", Name = "ObterComprasPorCliente")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterComprasPorCliente(Guid clienteId)
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.BuscarPorCliente(clienteId);

            var existeCompraDeOutraEmpresa = resultado.Any(c => c.EmpresaId != empresaId);

            if (existeCompraDeOutraEmpresa)
            {
                return Forbid("Você não tem permissão para acessar compras de outra empresa.");
            }

            return Ok(resultado ?? new List<CompraResponseDTO>());
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpGet("obterporempresa", Name = "ObterComprasPorEmpresa")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterComprasPorEmpresa()
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.BuscarPorEmpresa(empresaId);

            return Ok(resultado ?? new List<CompraResponseDTO>());
        }
        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpPost("criar", Name = "CriarCompra")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarCompra([FromBody] CompraCreateDTO dto)
        {
            var empresaId = User.ObterEmpresaId();

            var resultado = await _service.Criar(dto, empresaId);

            if (!resultado)
            {
                return BadRequest("Erro ao criar compra: " + resultado);
            }

            return Ok("Compra criada com sucesso.");
        }

        [Authorize(Policy = "Funcionario/Gerente")]
        [HttpPatch("atualizar/{id:guid}", Name = "AtualizarCompra")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarCompra(Guid id, [FromBody] CompraUpdateDTO dto)
        {
            var empresaId = User.ObterEmpresaId();

            var compra = await _service.BuscarPorId(id);

            if (compra == null)
            {
                return NotFound("Compra não encontrada.");
            }

            if (compra.EmpresaId != empresaId)
            {
                return Forbid("Você não tem permissão para atualizar esta compra.");
            }

            var resultado = await _service.Atualizar(id, dto);

            if (!resultado)
            {
                return BadRequest("Erro ao atualizar compra.");
            }

            return Ok("Compra atualizada com sucesso.");
        }

        [Authorize(Policy = "Gerente")]
        [HttpPatch("ativar/{id:guid}", Name = "AtivarCompra")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtivarCompra(Guid id)
        {
            var empresaId = User.ObterEmpresaId();

            var compra = await _service.BuscarPorId(id);

            if (compra == null)
            {
                return NotFound("Compra não encontrada.");
            }

            if (compra.EmpresaId != empresaId)
            {
                return Forbid("Você não tem permissão para ativar esta compra.");
            }

            var resultado = await _service.Ativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao ativar compra.");
            }

            return Ok("Compra ativada com sucesso.");
        }

        [Authorize(Policy = "Gerente")]
        [HttpDelete("desativar/{id:guid}", Name = "DesativarCompra")]
        [ProducesResponseType(typeof(CompraResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarCompra(Guid id)
        {
            var empresaId = User.ObterEmpresaId();

            var compra = await _service.BuscarPorId(id);

            if (compra == null)
            {
                return NotFound("Compra não encontrada.");
            }

            if (compra.EmpresaId != empresaId)
            {
                return Forbid("Você não tem permissão para desativar esta compra.");
            }

            var resultado = await _service.Desativar(id);

            if (!resultado)
            {
                return BadRequest("Erro ao desativar compra.");
            }

            return Ok("Compra desativada com sucesso.");
        }
    }
}