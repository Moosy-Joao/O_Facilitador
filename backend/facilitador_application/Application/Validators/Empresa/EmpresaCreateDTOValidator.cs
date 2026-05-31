using facilitador_application.Application.Validators.Utils;
using facilitador_application.Application.Validators.Utils.facilitador_application.Application.Validators.Utils;
using facilitador_domain.Domain.DTOs;
using FluentValidation;

namespace facilitador_application.Application.Validators.Empresa
{
    public class EmpresaCreateDTOValidator : AbstractValidator<EmpresaCreateDTO>
    {
        public EmpresaCreateDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .Must(ValidarEmail.Validar).WithMessage("O email deve ser válido.");
            RuleFor(x => x.CNPJ)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Must(ValidarDocumento.ValidarCPFouCNPJ).WithMessage("O CNPJ deve estar no formato válido.");
            RuleFor(x => x.Telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("O telefone deve conter apenas dígitos e pode incluir um '+' no início.");
        }
    }
}
