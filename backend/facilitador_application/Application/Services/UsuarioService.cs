using facilitador_api.Application.Interfaces;
using facilitador_api.Application.Mapping;
using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_domain.Domain.DTOs;
using facilitador_domain.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace facilitador_api.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IConfiguration _configuration;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IEmpresaRepository empresaRepository,
            IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
            _configuration = configuration;
        }

        public async Task<bool> Ativar(Guid id)
        {
            var usuario = await _usuarioRepository.Existe(id);
            if (!usuario)
            {
                return false;
            }

            await _usuarioRepository.Ativar(id);
            await _usuarioRepository.Salvar();

            return true;
        }

        public async Task<bool> Atualizar(Guid id, UsuarioUpdateDTO dto)
        {
            var usuario = await _usuarioRepository.BuscarPorId(id);
            if (usuario == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(dto.Nome))
            {
                usuario.AtualizarNome(dto.Nome);
            }

            if (!string.IsNullOrEmpty(dto.Email))
            {
                usuario.AtualizarEmail(dto.Email);
            }

            if (!string.IsNullOrEmpty(dto.Senha))
            {
                var senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
                usuario.AtualizarSenha(senhaHash);
            }

            // Necessário validar se o cargo é diferente de null antes de atualizar
            if (!Enum.Equals(dto.Cargo, default(CargoUsuario)))
            {
                usuario.AtualizarCargo((CargoUsuario)dto.Cargo);
            }

            // 5. Atualizar timestamp e salvar modificações
            //usuario.AtualizarModificadoEm(DateTime.UtcNow);
            await _usuarioRepository.Salvar();

            return true;
        }

        public async Task<UsuarioResponseDTO?> BuscarPorId(Guid id)
        {
            var usuario = await _usuarioRepository.BuscarPorId(id);
            return usuario?.ToResponseDTO();
        }

        public async Task<List<UsuarioResponseDTO>> BuscarUsuarios()
        {
            var usuarios = await _usuarioRepository.BuscarTodos();
            return usuarios.Select(c => c.ToResponseDTO()).ToList();
        }

        public async Task<LoginResponseDTO?> Criar(UsuarioCreateDTO dto)
        {
            var empresaExiste = await _empresaRepository.Existe(dto.EmpresaId);
            if (!empresaExiste)
            {
                return null;
            }

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var usuarioNovo = new Usuario(
                dto.EmpresaId,
                dto.Nome,
                dto.Email,
                senhaHash,
                dto.Cargo
            );

            await _usuarioRepository.Cadastrar(usuarioNovo);
            await _usuarioRepository.Salvar();

            var expirationMinutes = Convert.ToDouble(
                _configuration["Jwt:ExpirationMinutes"] ?? "120"
            );

            var expiraEm = DateTime.UtcNow.AddMinutes(expirationMinutes);
            var token = GerarToken(usuarioNovo.Id, usuarioNovo.Email, expiraEm);

            return new LoginResponseDTO
            {
                Token = token,
                ExpiraEm = expiraEm,
                UsuarioId = usuarioNovo.Id,
                Email = usuarioNovo.Email
            };
        }

        public async Task<bool> Desativar(Guid id)
        {
            var usuario = await _usuarioRepository.Existe(id);
            if (!usuario)
            {
                return false;
            }

            await _usuarioRepository.Desativar(id);
            await _usuarioRepository.Salvar();

            return true;
        }

        private string GerarToken(Guid usuarioId, string email, DateTime expiraEm)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT Key não configurada.");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("usuarioId", usuarioId.ToString())
            };

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expiraEm,
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}