using facilitador_api.Application.Interfaces;
using facilitador_api.Helpers;
using facilitador_application.Application.Validators.Usuario;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet("obter", Name = "ObterUsuarios")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsuarios()
        {
            var resultado = await _service.BuscarUsuarios();

            return Ok(resultado ?? new List<UsuarioResponseDTO>());
        }

        [HttpGet("obterporid/{id:guid}", Name = "ObterUsuarioPorId")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsuarioPorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);

            if (resultado == null)
            {
                return NotFound("Usuário não encontrado: " + resultado);
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Gerente")]
        [HttpPost("criar", Name = "CriarUsuario")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarUsuario(UsuarioCreateDTO dto)
        {
            var validador = new UsuarioCreateDTOValidator();
            var resultadoValidacao = validador.Validate(dto);

            if (!resultadoValidacao.IsValid)
            {
                return BadRequest(resultadoValidacao.Errors.Select(e => e.ErrorMessage));
            }

            var empresaIdToken = User.ObterEmpresaId();

            try
            {
                var resultado = await _service.Criar(dto, empresaIdToken);
                return CreatedAtAction(nameof(GetUsuarioPorId), resultado);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Erro ao criar usuário: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Erro ao criar usuário: " + ex.Message);
            }

        }

        [Authorize(Policy = "Gerente")]
        [HttpPatch("atualizar/{id:guid}", Name = "AtualizarUsuario")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarUsuario(Guid id, UsuarioUpdateDTO dto)
        {
            var validador = new UsuarioUpdateDTOValidator();
            var resultadoValidacao = validador.Validate(dto);

            if (!resultadoValidacao.IsValid)
            {
                return BadRequest(resultadoValidacao.Errors);
            }

            var resultado = await _service.Atualizar(id, dto);
            if (resultado == false)
            {
                return BadRequest("Erro ao atualizar usuário: " + resultado);
            }
            return Ok(resultado);
        }
        [Authorize(Policy = "Gerente")]
        [HttpPost("ativar/{id:guid}", Name = "AtivarUsuario")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtivarUsuario(Guid id)
        {
            var resultado = await _service.Ativar(id);

            if (resultado == false)
            {
                return BadRequest("Erro ao ativar usuário: " + resultado);
            }

            return Ok(resultado);
        }
        [Authorize(Policy = "Gerente")]
        [HttpDelete("desativar/{id:guid}", Name = "DesativarUsuario")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarUsuario(Guid id)
        {
            var resultado = await _service.Desativar(id);

            if (resultado == false)
            {
                return BadRequest("Erro ao desativar usuário: " + resultado);
            }

            return Ok(resultado);
        }
    }
}
