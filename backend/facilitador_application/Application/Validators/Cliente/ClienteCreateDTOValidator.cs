using facilitador_application.Application.Validators.Utils.facilitador_application.Application.Validators.Utils;
using facilitador_application.Application.Validators.Utils;
using facilitador_domain.Domain.DTOs;
using FluentValidation;

namespace facilitador_application.Application.Validators.Cliente
{
    public class ClienteCreateDTOValidator : AbstractValidator<ClienteCreateDTO>
    {
        public ClienteCreateDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .Must(ValidarEmail.Validar).WithMessage("O email deve ser válido.");
            RuleFor(x => x.Documento)
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Must(ValidarDocumento.ValidarCPFouCNPJ).WithMessage("O Documento deve estar no formato de CPF ou CNPJ.");
            RuleFor(x => x.Telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("O telefone deve conter apenas dígitos e pode incluir um '+' no início.");
            RuleFor(x => x.LimiteCredito)
                .GreaterThanOrEqualTo(0).WithMessage("O limite de crédito deve ser maior ou igual a zero.")
                .LessThanOrEqualTo(99999999.99M).WithMessage("O limite de crédito deve ser menor ou igual a 99.999.999,99.");
            RuleFor(x => x.Saldo)
                .GreaterThanOrEqualTo(0).WithMessage("O saldo deve ser maior ou igual a zero.")
                .LessThanOrEqualTo(99999999.99M).WithMessage("O saldo deve ser menor ou igual a 99.999.999,99.");
        }
    }
}