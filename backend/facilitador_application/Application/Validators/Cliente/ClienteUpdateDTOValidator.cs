using facilitador_application.Application.Validators.Utils;
using facilitador_application.Application.Validators.Utils.facilitador_application.Application.Validators.Utils;
using facilitador_domain.Domain.DTOs;
using FluentValidation;

namespace facilitador_application.Application.Validators.Cliente
{
    public class ClienteUpdateDTOValidator : AbstractValidator<ClienteUpdateDTO>
    {
        public ClienteUpdateDTOValidator()
        {
            RuleFor(x => x.Nome)
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.")
                .When(x => x.Nome != null);
            RuleFor(x => x.Email)
                .Must(ValidarEmail.Validar).WithMessage("O email deve ser válido.")
                .When(x => x.Email != null);
            RuleFor(x => x.Documento)
                .Must(ValidarDocumento.ValidarCPFouCNPJ).WithMessage("O Documento deve estar no formato de CPF ou CNPJ.")
                .When(x => x.Documento != null);
            RuleFor(x => x.Telefone)
                .Matches(@"^\+?\d{10,15}$").WithMessage("O telefone deve conter apenas dígitos e pode incluir um '+' no início.")
                .When(x => x.Telefone != null);
            RuleFor(x => x.Saldo)
                .GreaterThanOrEqualTo(0).WithMessage("O saldo deve ser maior ou igual a zero.")
                .LessThanOrEqualTo(99999999.99M).WithMessage("O saldo deve ser menor ou igual a 99.999.999,99.")
                .When(x => x.Saldo != null);
            RuleFor(x => x.LimiteCredito)
                .GreaterThanOrEqualTo(0).WithMessage("O limite de crédito deve ser maior ou igual a zero.")
                .LessThanOrEqualTo(99999999.99M).WithMessage("O limite de crédito deve ser menor ou igual a 99.999.999,99.")
                .When(x => x.LimiteCredito != null);
        }
    }
}
