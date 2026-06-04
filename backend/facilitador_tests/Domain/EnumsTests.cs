using facilitador_domain.Domain.Enums;

namespace facilitador_tests.Domain;

/// <summary>
/// CAMADA 2 — Domain / Enums
/// Testa os valores dos enums de domínio.
/// </summary>
public class EnumsTests
{
    [Fact(DisplayName = "CargoUsuario: Administrador = 0")]
    public void Administrador_DeveSerZero()
    {
        Assert.Equal(0, (int)CargoUsuario.Administrador);
    }

    [Fact(DisplayName = "CargoUsuario: Gerente = 1")]
    public void Gerente_DeveSerUm()
    {
        Assert.Equal(1, (int)CargoUsuario.Gerente);
    }

    [Fact(DisplayName = "CargoUsuario: Funcionario = 2")]
    public void Funcionario_DeveSerDois()
    {
        Assert.Equal(2, (int)CargoUsuario.Funcionario);
    }

    [Fact(DisplayName = "CargoUsuario: Existem exatamente 3 cargos")]
    public void CargoUsuario_DeveTer3Valores()
    {
        var valores = Enum.GetValues<CargoUsuario>();
        Assert.Equal(3, valores.Length);
    }

    [Theory(DisplayName = "CargoUsuario: Todos os valores são válidos via Enum.IsDefined")]
    [InlineData(CargoUsuario.Administrador)]
    [InlineData(CargoUsuario.Gerente)]
    [InlineData(CargoUsuario.Funcionario)]
    public void TodosOsCargos_SaoValidos(CargoUsuario cargo)
    {
        Assert.True(Enum.IsDefined(cargo));
    }

    [Fact(DisplayName = "CargoUsuario: Valor 99 não é um cargo válido")]
    public void ValorInvalido_NaoEhDefinido()
    {
        Assert.False(Enum.IsDefined((CargoUsuario)99));
    }
}
