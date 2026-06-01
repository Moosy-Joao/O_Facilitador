using facilitador_application.Application.Validators.Utils;
using facilitador_application.Application.Validators.Utils.facilitador_application.Application.Validators.Utils;
using facilitador_domain.Domain.DTOs;
using FluentValidation;

namespace facilitador_application.Application.Validators.Empresa
{
    public class EmpresaUpdateDTOValidator : AbstractValidator<EmpresaUpdateDTO>
    {
        public EmpresaUpdateDTOValidator()
        {
            RuleFor(x => x.Nome)
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.")
                .When(x => x.Nome != null);
            RuleFor(x => x.Email)
                .Must(ValidarEmail.Validar).WithMessage("O email deve ser válido.")
                .When(x => x.Email != null);
            RuleFor(x => x.CNPJ)
                .Must(ValidarDocumento.ValidarCPFouCNPJ).WithMessage("O CNPJ deve estar no formato válido.")
                .When(x => x.CNPJ != null);
            RuleFor(x => x.Telefone)
                .Matches(@"^\+?\d{10,15}$").WithMessage("O telefone deve conter apenas dígitos e pode incluir um '+' no início.")
                .When(x => x.Telefone != null);
        }
    }
}
