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

        [Authorize(Policy = "Gerente")]
        [HttpGet("obter", Name = "ObterUsuarios")]
        [ProducesResponseType(typeof(List<UsuarioResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsuarios()
        {
            var resultado = await _service.BuscarUsuarios();

            return Ok(resultado ?? new List<UsuarioResponseDTO>());
        }

        [Authorize(Policy = "Gerente")]
        [HttpGet("obterporid/{id:guid}", Name = "ObterUsuarioPorId")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsuarioPorId(Guid id)
        {
            var resultado = await _service.BuscarPorId(id);

            if (resultado == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Gerente")]
        [HttpPost("criar", Name = "CriarUsuario")]
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CriarUsuario([FromBody] UsuarioCreateDTO dto)
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

                if (resultado == null)
                {
                    return BadRequest("Erro ao criar usuário.");
                }

                return Ok(resultado);
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
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarUsuario(Guid id, [FromBody] UsuarioUpdateDTO dto)
        {
            var validador = new UsuarioUpdateDTOValidator();
            var resultadoValidacao = validador.Validate(dto);

            if (!resultadoValidacao.IsValid)
            {
                return BadRequest(resultadoValidacao.Errors.Select(e => e.ErrorMessage));
            }

            var resultado = await _service.Atualizar(id, dto);

            if (resultado == false)
            {
                return BadRequest("Erro ao atualizar usuário.");
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Gerente")]
        [HttpPost("ativar/{id:guid}", Name = "AtivarUsuario")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtivarUsuario(Guid id)
        {
            var resultado = await _service.Ativar(id);

            if (resultado == false)
            {
                return BadRequest("Erro ao ativar usuário.");
            }

            return Ok(resultado);
        }

        [Authorize(Policy = "Gerente")]
        [HttpDelete("desativar/{id:guid}", Name = "DesativarUsuario")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarUsuario(Guid id)
        {
            var resultado = await _service.Desativar(id);

            if (resultado == false)
            {
                return BadRequest("Erro ao desativar usuário.");
            }

            return Ok(resultado);
        }
    }
}