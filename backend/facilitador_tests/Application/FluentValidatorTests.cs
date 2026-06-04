using facilitador_application.Application.Validators.Cliente;
using facilitador_application.Application.Validators.Empresa;
using facilitador_application.Application.Validators.Usuario;
using facilitador_domain.Domain.DTOs;
using facilitador_domain.Domain.Enums;

namespace facilitador_tests.Application;

public class FluentValidatorTests
{
    // ── ClienteCreateDTO ──
    [Fact(DisplayName = "ClienteCreate: DTO válido passa")]
    public void ClienteCreate_Valido()
    {
        var dto = new ClienteCreateDTO
        {
            Nome = "João Silva",
            Email = "joao@email.com",
            Documento = "52998224725",
            Telefone = "11999990000",
            Saldo = 0,
            LimiteCredito = 500,
            EnderecoId = Guid.NewGuid(),
            EmpresaId = Guid.NewGuid()
        };
        var result = new ClienteCreateDTOValidator().Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "ClienteCreate: Nome vazio falha")]
    public void ClienteCreate_NomeVazio()
    {
        var dto = new ClienteCreateDTO
        {
            Nome = "",
            Email = "j@e.com",
            Documento = "52998224725",
            Telefone = "11999990000",
            Saldo = 0,
            LimiteCredito = 500
        };
        var result = new ClienteCreateDTOValidator().Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Nome");
    }

    [Fact(DisplayName = "ClienteCreate: Email inválido falha")]
    public void ClienteCreate_EmailInvalido()
    {
        var dto = new ClienteCreateDTO
        {
            Nome = "Test",
            Email = "invalido",
            Documento = "52998224725",
            Telefone = "11999990000",
            Saldo = 0,
            LimiteCredito = 500
        };
        var result = new ClienteCreateDTOValidator().Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact(DisplayName = "ClienteCreate: CPF inválido falha")]
    public void ClienteCreate_CPFInvalido()
    {
        var dto = new ClienteCreateDTO
        {
            Nome = "Test",
            Email = "t@e.com",
            Documento = "00000000000",
            Telefone = "11999990000",
            Saldo = 0,
            LimiteCredito = 500
        };
        var result = new ClienteCreateDTOValidator().Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Documento");
    }

    [Fact(DisplayName = "ClienteCreate: Saldo negativo falha")]
    public void ClienteCreate_SaldoNegativo()
    {
        var dto = new ClienteCreateDTO
        {
            Nome = "Test",
            Email = "t@e.com",
            Documento = "52998224725",
            Telefone = "11999990000",
            Saldo = -10,
            LimiteCredito = 500
        };
        var result = new ClienteCreateDTOValidator().Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Saldo");
    }

    // ── EmpresaCreateDTO ──
    [Fact(DisplayName = "EmpresaCreate: DTO válido passa")]
    public void EmpresaCreate_Valido()
    {
        var dto = new EmpresaCreateDTO
        {
            Nome = "Loja Central",
            Email = "loja@central.com",
            CNPJ = "11222333000181",
            EnderecoId = Guid.NewGuid()
        };
        var result = new EmpresaCreateDTOValidator().Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "EmpresaCreate: CNPJ inválido falha")]
    public void EmpresaCreate_CNPJInvalido()
    {
        var dto = new EmpresaCreateDTO
        {
            Nome = "Loja",
            Email = "l@l.com",
            CNPJ = "00000000000000"
        };
        var result = new EmpresaCreateDTOValidator().Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CNPJ");
    }

    // ── UsuarioCreateDTO ──
    [Fact(DisplayName = "UsuarioCreate: DTO válido passa")]
    public void UsuarioCreate_Valido()
    {
        var dto = new UsuarioCreateDTO
        {
            Nome = "Ana Maria",
            Email = "ana@email.com",
            Senha = "Senha@123",
            Cargo = CargoUsuario.Gerente,
            EmpresaId = Guid.NewGuid()
        };
        var result = new UsuarioCreateDTOValidator().Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "UsuarioCreate: Senha fraca falha")]
    public void UsuarioCreate_SenhaFraca()
    {
        var dto = new UsuarioCreateDTO
        {
            Nome = "Ana",
            Email = "a@a.com",
            Senha = "123",
            Cargo = CargoUsuario.Funcionario,
            EmpresaId = Guid.NewGuid()
        };
        var result = new UsuarioCreateDTOValidator().Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha");
    }

    [Fact(DisplayName = "UsuarioCreate: EmpresaId vazio falha")]
    public void UsuarioCreate_EmpresaVazia()
    {
        var dto = new UsuarioCreateDTO
        {
            Nome = "Ana",
            Email = "a@a.com",
            Senha = "Senha@123",
            Cargo = CargoUsuario.Funcionario,
            EmpresaId = Guid.Empty
        };
        var result = new UsuarioCreateDTOValidator().Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "EmpresaId");
    }
}
