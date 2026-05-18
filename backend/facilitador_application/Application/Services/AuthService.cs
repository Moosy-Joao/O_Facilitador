using facilitador_api.Application.Interfaces;
using facilitador_api.Domain.Interfaces;
using facilitador_domain.Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace facilitador_api.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO?> Login(LoginDTO dto)
        {
            var usuario = await _usuarioRepository.BuscarPorEmail(dto.Email);

            if (usuario == null)
            {
                return null;
            }

            // Temporário. Depois vamos trocar por BCrypt.
            if (usuario.Senha != dto.Senha)
            {
                return null;
            }

            var expirationMinutes = Convert.ToDouble(
                _configuration["Jwt:ExpirationMinutes"] ?? "120"
            );

            var expiraEm = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = GerarToken(usuario.Id, usuario.Email, expiraEm);

            return new LoginResponseDTO
            {
                Token = token,
                ExpiraEm = expiraEm,
                UsuarioId = usuario.Id,
                Email = usuario.Email
            };
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