using System.ComponentModel.DataAnnotations;

namespace facilitador_domain.Domain.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Senha' é obrigatório.")]
        public string Senha { get; set; } = string.Empty;
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
        public Guid UsuarioId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}