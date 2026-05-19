using facilitador_application.Application.Validators.Utils;
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
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("O email deve ser válido.");
            RuleFor(x => x.Documento)
                .Must(ValidarDocumento.ValidarCPFouCNPJ).WithMessage("O Documento deve estar no formato de CPF ou CNPJ.");
            RuleFor(x => x.Telefone)
                .Matches(@"^\+?\d{10,15}$").WithMessage("O telefone deve conter apenas dígitos e pode incluir um '+' no início.");
            RuleFor(x => x.Saldo)
                .GreaterThanOrEqualTo(0).WithMessage("O saldo deve ser maior ou igual a zero.");
            RuleFor(x => x.LimiteCredito)
                .GreaterThanOrEqualTo(0).WithMessage("O limite de crédito deve ser maior ou igual a zero.");
        }
    }
}
