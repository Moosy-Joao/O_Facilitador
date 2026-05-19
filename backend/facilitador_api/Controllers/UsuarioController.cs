using facilitador_api.Application.Interfaces;
using facilitador_application.Application.Validators.Usuario;
using facilitador_domain.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.Controllers
{
    [ApiController]
    [Route("api/v1/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet(Name = "ObterUsuarios")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _service.BuscarUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id:guid}", Name = "ObterUsuarioPorId")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsuarioPorId(Guid id)
        {
            var usuario = await _service.BuscarPorId(id);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }
            return Ok(usuario);
        }

        [HttpPost(Name = "CriarUsuario")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarUsuario(UsuarioCreateDTO dto)
        {
            var validador = new UsuarioCreateDTOValidator();
            var resultadoValidacao = validador.Validate(dto);

            if (!resultadoValidacao.IsValid)
            {
                return BadRequest(resultadoValidacao.Errors);
            }

            var resultado = await _service.Criar(dto);
            if (resultado == false)
            {
                return BadRequest("Erro ao criar usuário." + true);
            }
            return Ok(resultado);
        }

        [HttpPatch("{id:guid}", Name = "AtualizarUsuario")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarUsuario(Guid id, UsuarioUpdateDTO dto)
        {


            var resultado = await _service.Atualizar(id, dto);
            if (resultado == false)
            {
                return BadRequest("Erro ao atualizar usuário.");
            }
            return Ok(resultado);
        }

        [HttpPost("{id:guid}", Name = "AtivarUsuario")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
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

        [HttpDelete("{id:guid}", Name = "DesativarUsuario")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
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
