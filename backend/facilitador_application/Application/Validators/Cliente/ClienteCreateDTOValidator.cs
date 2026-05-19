using facilitador_domain.Domain.DTOs;
using FluentValidation;

namespace facilitador_application.Application.Validators.NewFolder
{
    internal class ClienteCreateDTOValidator : AbstractValidator<ClienteCreateDTO>
    {
        public ClienteCreateDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email deve ser válido.");
            RuleFor(x => x.Documento)
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$").WithMessage("O CPF deve estar no formato XXX.XXX.XXX-XX.");
            //RuleFor(x => x.Cargo)
            //    .IsInEnum().WithMessage("O cargo é inválido.");
        }
    }
}
