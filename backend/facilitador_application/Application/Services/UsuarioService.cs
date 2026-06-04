using facilitador_api.Application.Interfaces;
using facilitador_api.Application.Mapping;
using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_domain.Domain.DTOs;
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
            if (dto.Cargo.HasValue)
            {
                usuario.AtualizarCargo(dto.Cargo.Value);
            }

            // 5. Atualizar timestamp e salvar modificações
            usuario.AtualizarModificadoEm(DateTime.UtcNow);
            await _usuarioRepository.Salvar();

            return true;
        }

        public async Task<UsuarioResponseDTO?> BuscarPorId(Guid id)
        {
            var usuario = await _usuarioRepository.BuscarPorId(id);
            return usuario?.ToResponseDTO();
        }

        public async Task<List<UsuarioResponseDTO>> BuscarUsuarios(Guid? empresaId = null)
        {
            List<Usuario> usuarios;
            if (empresaId.HasValue && empresaId.Value != Guid.Empty)
            {
                usuarios = await _usuarioRepository.BuscarPorEmpresa(empresaId.Value);
            }
            else
            {
                usuarios = await _usuarioRepository.BuscarTodos();
            }
            return usuarios.Select(c => c.ToResponseDTO()).ToList();
        }

        public async Task<LoginResponseDTO?> Criar(UsuarioCreateDTO dto, Guid empresaId)
        {
            var empresaExiste = await _empresaRepository.Existe(dto.EmpresaId);
            if (!empresaExiste)
            {
                return null;
            }

            if (empresaId == Guid.Empty)
            {
                var usuariosDaEmpresa = await _usuarioRepository.BuscarPorEmpresa(dto.EmpresaId);
                if (usuariosDaEmpresa.Any())
                {
                    throw new UnauthorizedAccessException("O usuário não tem permissão para criar um usuário para esta empresa.");
                }
            }
            else if (dto.EmpresaId != empresaId)
            {
                throw new UnauthorizedAccessException("O usuário não tem permissão para criar um usuário para esta empresa.");
            }

            var usuarioExistente = await _usuarioRepository.BuscarPorEmail(dto.Email);
            if (usuarioExistente != null)
            {
                throw new InvalidOperationException("Já existe um usuário cadastrado com este e-mail.");
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
            var token = GerarToken(usuarioNovo.Id, usuarioNovo.EmpresaId, usuarioNovo.Email, usuarioNovo.Cargo.ToString(), expiraEm);

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

        private string GerarToken(Guid usuarioId, Guid empresaId, string email, string cargo, DateTime expiraEm)
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
                new Claim("usuarioId", usuarioId.ToString()),
                new Claim("empresaId", empresaId.ToString()),
                new Claim("cargo", cargo),
                new Claim(ClaimTypes.Role, cargo)
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

        public async Task<bool> EsqueciSenha(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var usuario = await _usuarioRepository.BuscarPorEmail(email);
            if (usuario == null)
            {
                // Para evitar enumeração de e-mails, retornamos verdadeiro simulando sucesso
                return true;
            }

            // Gerar token de redefinição de 15 minutos
            var expiraEm = DateTime.UtcNow.AddMinutes(15);
            var token = GerarTokenRedefinicao(usuario.Email, expiraEm);

            // Link do frontend para resetar senha
            var resetUrl = $"http://localhost:5173/resetar-senha?token={token}";

            // Dispara o envio de e-mail (SMTP físico ou mock no Console)
            EnviarEmailRecuperacao(usuario.Email, usuario.Nome, resetUrl);

            return true;
        }

        private string GerarTokenRedefinicao(string email, DateTime expiraEm)
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
                new Claim("resetEmail", email),
                new Claim("purpose", "password_reset")
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

        public async Task<bool> ResetarSenha(string token, string novaSenha)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(novaSenha))
            {
                return false;
            }

            try
            {
                var jwtKey = _configuration["Jwt:Key"];
                var jwtIssuer = _configuration["Jwt:Issuer"];
                var jwtAudience = _configuration["Jwt:Audience"];

                var tokenHandler = new JwtSecurityTokenHandler();
                var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

                var parametrosValidacao = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = chave,
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, parametrosValidacao, out SecurityToken validatedToken);
                
                var purposeClaim = principal.FindFirst("purpose")?.Value;
                if (purposeClaim != "password_reset")
                {
                    return false;
                }

                var email = principal.FindFirst("resetEmail")?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return false;
                }

                var usuario = await _usuarioRepository.BuscarPorEmail(email);
                if (usuario == null)
                {
                    return false;
                }

                var senhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);
                usuario.AtualizarSenha(senhaHash);
                usuario.AtualizarModificadoEm(DateTime.UtcNow);

                await _usuarioRepository.Salvar();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RESET SENHA] Falha na validação do token: {ex.Message}");
                return false;
            }
        }

        private void EnviarEmailRecuperacao(string email, string nome, string resetUrl)
        {
            var body = $@"
                <div style='font-family: sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e5e7eb; border-radius: 12px;'>
                    <h2 style='color: #16a34a;'>Olá, {nome}!</h2>
                    <p style='color: #4b5548; font-size: 15px;'>Recebemos uma solicitação de redefinição de senha para sua conta no <strong>O Facilitador</strong>.</p>
                    <p style='color: #4b5548; font-size: 15px;'>Clique no botão abaixo para redefinir a sua senha (este link expira em 15 minutos):</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetUrl}' target='_blank' style='background-color: #16a34a; color: white; padding: 12px 24px; text-decoration: none; border-radius: 8px; font-weight: bold; display: inline-block; box-shadow: 0 4px 6px -1px rgba(22, 163, 74, 0.2);'>Redefinir Minha Senha</a>
                    </div>
                    <p style='color: #ef4444; font-size: 14px;'>Se o botão acima não funcionar, copie e cole o link abaixo no seu navegador:</p>
                    <p style='word-break: break-all; color: #3b82f6; font-size: 13px;'>{resetUrl}</p>
                    <hr style='border: none; border-top: 1px solid #e5e7eb; margin: 20px 0;' />
                    <p style='color: #94a392; font-size: 12px;'>Atenciosamente,<br/>Equipe O Facilitador</p>
                </div>";

            try
            {
                var host = _configuration["Smtp:Host"] ?? "smtp.gmail.com";
                var portStr = _configuration["Smtp:Port"];
                var port = string.IsNullOrEmpty(portStr) ? 587 : int.Parse(portStr);
                var username = _configuration["Smtp:Username"];
                var password = _configuration["Smtp:Password"];
                var fromAddress = _configuration["Smtp:FromAddress"] ?? "no-reply@ofacilitador.com.br";

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    // Fallback silencioso de desenvolvimento
                    Console.WriteLine("\n==================================================");
                    Console.WriteLine($"[EMAIL MOCK] Destinatário: {email}");
                    Console.WriteLine($"[EMAIL MOCK] Assunto: Redefinição de Senha - O Facilitador");
                    Console.WriteLine($"[EMAIL MOCK] Clique no Link abaixo para redefinir sua senha:");
                    Console.WriteLine($"[EMAIL MOCK] Link: {resetUrl}");
                    Console.WriteLine("==================================================\n");
                    return;
                }

                using (var mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new System.Net.Mail.MailAddress(fromAddress, "O Facilitador");
                    mail.To.Add(email);
                    mail.Subject = "Redefinição de Senha - O Facilitador";
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (var smtp = new System.Net.Mail.SmtpClient(host, port))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(username, password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                
                Console.WriteLine($"[SMTP] E-mail de redefinição enviado com sucesso para {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n==================================================");
                Console.WriteLine($"[SMTP FALHA] Erro ao enviar e-mail: {ex.Message}");
                Console.WriteLine($"[SMTP FALLBACK] E-mail: {email}");
                Console.WriteLine($"[SMTP FALLBACK] Link: {resetUrl}");
                Console.WriteLine("==================================================\n");
            }
        }
    }
}