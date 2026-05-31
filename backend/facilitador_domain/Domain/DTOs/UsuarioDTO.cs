using facilitador_domain.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace facilitador_domain.Domain.DTOs
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo 'Nome' deve conter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Senha' é obrigatório.")]
        public string Senha { get; set; } = string.Empty;
    }

    public class UsuarioCreateDTO : UsuarioDTO
    {
        [Required(ErrorMessage = "O campo 'Cargo' é obrigatório.")]
        public CargoUsuario Cargo { get; set; } = CargoUsuario.Funcionario;

        [Required(ErrorMessage = "O campo 'Empresa' é obrigatório.")]
        public Guid EmpresaId { get; set; } = Guid.Empty;
    }

    public class UsuarioUpdateDTO
    {
        [StringLength(100, ErrorMessage = "O campo 'Nome' deve conter no máximo 100 caracteres.")]
        public string? Nome { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public CargoUsuario? Cargo { get; set; }
    }

    public class UsuarioResponseDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public CargoUsuario Cargo { get; set; }
        public Guid EmpresaId { get; set; }
        public EmpresaResponseDTO? Empresa { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime ModificadoEm { get; set; }

    }

    public class EsqueciSenhaDTO
    {
        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetarSenhaDTO
    {
        [Required(ErrorMessage = "O token de redefinição é obrigatório.")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string NovaSenha { get; set; } = string.Empty;
    }
}
