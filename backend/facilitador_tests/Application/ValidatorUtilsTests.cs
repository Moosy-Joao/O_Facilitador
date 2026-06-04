using facilitador_application.Application.Validators.Utils;
using facilitador_application.Application.Validators.Utils.facilitador_application.Application.Validators.Utils;

namespace facilitador_tests.Application;

public class ValidatorUtilsTests
{
    // CPF
    [Theory(DisplayName = "CPF válido")]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    public void CPFValido(string cpf) => Assert.True(ValidarDocumento.ValidarCPFouCNPJ(cpf));

    [Theory(DisplayName = "CPF inválido")]
    [InlineData("111.111.111-11")]
    [InlineData("123.456.789-00")]
    [InlineData("12345")]
    [InlineData("")]
    public void CPFInvalido(string cpf) => Assert.False(ValidarDocumento.ValidarCPFouCNPJ(cpf));

    [Fact(DisplayName = "CPF null")]
    public void CPFNull() => Assert.False(ValidarDocumento.ValidarCPFouCNPJ(null!));

    // CNPJ
    [Theory(DisplayName = "CNPJ válido")]
    [InlineData("11.222.333/0001-81")]
    [InlineData("11222333000181")]
    public void CNPJValido(string cnpj) => Assert.True(ValidarDocumento.ValidarCPFouCNPJ(cnpj));

    [Theory(DisplayName = "CNPJ inválido")]
    [InlineData("11.111.111/1111-11")]
    [InlineData("00.000.000/0000-00")]
    public void CNPJInvalido(string cnpj) => Assert.False(ValidarDocumento.ValidarCPFouCNPJ(cnpj));

    // Email
    [Theory(DisplayName = "Email válido")]
    [InlineData("user@example.com")]
    [InlineData("nome.sobrenome@empresa.com.br")]
    [InlineData("a@b.co")]
    public void EmailValido(string email) => Assert.True(ValidarEmail.Validar(email));

    [Theory(DisplayName = "Email inválido")]
    [InlineData("")]
    [InlineData("semArroba")]
    [InlineData("@semlocal.com")]
    [InlineData("user@")]
    [InlineData("user@.com")]
    public void EmailInvalido(string email) => Assert.False(ValidarEmail.Validar(email));

    [Fact(DisplayName = "Email null")]
    public void EmailNull() => Assert.False(ValidarEmail.Validar(null!));

    // Senha
    [Theory(DisplayName = "Senha forte")]
    [InlineData("Senha@123")]
    [InlineData("Abc!1def")]
    public void SenhaForte(string senha) => Assert.True(ValidarSenha.Validar(senha));

    [Theory(DisplayName = "Senha fraca")]
    [InlineData("senha123")]
    [InlineData("SENHA123")]
    [InlineData("Abc123")]
    [InlineData("")]
    public void SenhaFraca(string senha) => Assert.False(ValidarSenha.Validar(senha));

    [Fact(DisplayName = "Senha null")]
    public void SenhaNull() => Assert.False(ValidarSenha.Validar(null!));
}
