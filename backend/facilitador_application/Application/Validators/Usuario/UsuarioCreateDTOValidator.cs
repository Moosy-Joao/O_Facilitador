using facilitador_application.Application.Validators.Utils;
using facilitador_application.Application.Validators.Utils.facilitador_application.Application.Validators.Utils;
using facilitador_domain.Domain.DTOs;
using FluentValidation;

namespace facilitador_application.Application.Validators.Usuario
{
    public class UsuarioCreateDTOValidator : AbstractValidator<UsuarioCreateDTO>
    {
        public UsuarioCreateDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .Must(ValidarEmail.Validar).WithMessage("O email deve ser válido.");
            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .Must(ValidarSenha.Validar).WithMessage("A senha deve conter letras maiúsculas, minúsculas, números e caracteres especiais.")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
                .MaximumLength(254).WithMessage("A senha deve ter no máximo 254 caracteres.");
            RuleFor(x => x.Cargo)
                .IsInEnum().WithMessage("O cargo é inválido.");
            RuleFor(x => x.EmpresaId)
                .NotEqual(Guid.Empty).WithMessage("A empresa é obrigatória.");
        }
    }
}
