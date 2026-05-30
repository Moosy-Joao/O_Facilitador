using facilitador_application.Application.Validators.Utils.facilitador_application.Application.Validators.Utils;
using facilitador_domain.Domain.DTOs;
using FluentValidation;

namespace facilitador_application.Application.Validators.Usuario
{
    public class UsuarioUpdateDTOValidator : AbstractValidator<UsuarioUpdateDTO>
    {
        public UsuarioUpdateDTOValidator()
        {
            RuleFor(x => x.Nome)
                .MaximumLength(100)
                .WithMessage("O campo 'Nome' deve conter no máximo 100 caracteres.");
            RuleFor(x => x.Email)
                .Must(ValidarEmail.Validar).WithMessage("O email deve ser válido.")
                .WithMessage("O campo 'Email' deve ser um endereço de email válido.");
            RuleFor(x => x.Senha)
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
                .MaximumLength(254).WithMessage("A senha deve ter no máximo 254 caracteres.");
            RuleFor(x => x.Cargo)
                .IsInEnum().WithMessage("O cargo é inválido.");
        }
    }
}
