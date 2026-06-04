using facilitador_api.Domain.Entities;
using facilitador_domain.Domain.DTOs;
using facilitador_domain.Domain.Enums;

namespace facilitador_tests.Domain;

/// <summary>
/// CAMADA 1 — Domain / Entities
/// Testa a criação e mutações das entidades de domínio.
/// </summary>
public class EntidadesTests
{
    // ────────────── BaseModel ──────────────

    [Fact(DisplayName = "BaseModel: Nova entidade nasce ativa")]
    public void NovaEntidade_DeveNascerAtiva()
    {
        var cliente = new Cliente(Guid.NewGuid(), "João", "j@email.com", "12345678901", "11999990000",
            Guid.NewGuid(), 0, 500, false);

        Assert.True(cliente.Ativo);
    }

    [Fact(DisplayName = "BaseModel: CriadoEm é preenchido automaticamente")]
    public void NovaEntidade_DeveTerCriadoEm()
    {
        var antes = DateTime.UtcNow.AddSeconds(-1);
        var endereco = new Endereco("Brasil", "SP", "São Paulo", "Centro", "Rua A", "100", "01000000");
        var depois = DateTime.UtcNow.AddSeconds(1);

        Assert.InRange(endereco.CriadoEm, antes, depois);
    }

    [Fact(DisplayName = "BaseModel: Desativar marca Ativo=false")]
    public void Desativar_DeveMarcarComoInativo()
    {
        var endereco = new Endereco("Brasil", "SP", "São Paulo", "Centro", "Rua A", "100", "01000000");
        endereco.Desativar();
        Assert.False(endereco.Ativo);
    }

    [Fact(DisplayName = "BaseModel: Ativar após Desativar restaura Ativo=true")]
    public void Ativar_AposDesativar_DeveRestaurar()
    {
        var endereco = new Endereco("Brasil", "SP", "São Paulo", "Centro", "Rua A", "100", "01000000");
        endereco.Desativar();
        endereco.Ativar();
        Assert.True(endereco.Ativo);
    }

    [Fact(DisplayName = "BaseModel: AtualizarModificadoEm altera a data")]
    public void AtualizarModificadoEm_DeveAlterarData()
    {
        var endereco = new Endereco("Brasil", "SP", "São Paulo", "Centro", "Rua A", "100", "01000000");
        var novaData = new DateTime(2025, 1, 1, 12, 0, 0);
        endereco.AtualizarModificadoEm(novaData);
        Assert.Equal(novaData, endereco.ModificadoEm);
    }

    // ────────────── Cliente ──────────────

    [Fact(DisplayName = "Cliente: Construtor com parâmetros preenche todos os campos")]
    public void ClienteConstrutor_PreencheTodosOsCampos()
    {
        var empresaId = Guid.NewGuid();
        var enderecoId = Guid.NewGuid();

        var cliente = new Cliente(empresaId, "Maria", "maria@email.com", "12345678901", "11999990000",
            enderecoId, 100, 500, false);

        Assert.Equal("Maria", cliente.Nome);
        Assert.Equal("maria@email.com", cliente.Email);
        Assert.Equal("12345678901", cliente.Documento);
        Assert.Equal("11999990000", cliente.Telefone);
        Assert.Equal(100, cliente.Saldo);
        Assert.Equal(500, cliente.LimiteCredito);
        Assert.Equal(empresaId, cliente.EmpresaId);
        Assert.Equal(enderecoId, cliente.EnderecoId);
        Assert.False(cliente.Inadimplente);
    }

    [Fact(DisplayName = "Cliente: Construtor com DTO preenche campos corretamente")]
    public void ClienteConstrutorDTO_PreencheCampos()
    {
        var dto = new ClienteCreateDTO
        {
            Nome = "Pedro",
            Email = "pedro@email.com",
            Documento = "98765432100",
            Telefone = "21988880000",
            Saldo = 50,
            LimiteCredito = 200,
            EnderecoId = Guid.NewGuid(),
            EmpresaId = Guid.NewGuid()
        };

        var empresaId = Guid.NewGuid();
        var enderecoId = Guid.NewGuid();
        var cliente = new Cliente(dto, empresaId, enderecoId);

        Assert.Equal("Pedro", cliente.Nome);
        Assert.Equal("pedro@email.com", cliente.Email);
        Assert.Equal(empresaId, cliente.EmpresaId);
        Assert.Equal(enderecoId, cliente.EnderecoId);
    }

    [Fact(DisplayName = "Cliente: Mutações atualizam os campos")]
    public void ClienteMutacoes_AtualizamCampos()
    {
        var cliente = new Cliente(Guid.NewGuid(), "A", "a@a.com", "11111111111", "11111111111",
            Guid.NewGuid(), 0, 100, false);

        cliente.AtualizarNome("Novo Nome");
        cliente.AtualizarEmail("novo@email.com");
        cliente.AtualizarDocumento("99999999999");
        cliente.AtualizarTelefone("22222222222");
        cliente.AtualizarSaldo(300);
        cliente.AtualizarLimiteCredito(1000);
        cliente.AtualizarInadimplente(true);

        Assert.Equal("Novo Nome", cliente.Nome);
        Assert.Equal("novo@email.com", cliente.Email);
        Assert.Equal("99999999999", cliente.Documento);
        Assert.Equal("22222222222", cliente.Telefone);
        Assert.Equal(300, cliente.Saldo);
        Assert.Equal(1000, cliente.LimiteCredito);
        Assert.True(cliente.Inadimplente);
    }

    // ────────────── Empresa ──────────────

    [Fact(DisplayName = "Empresa: Construtor preenche campos")]
    public void EmpresaConstrutor_PreencheCampos()
    {
        var enderecoId = Guid.NewGuid();
        var empresa = new Empresa("Loja X", "12345678000199", "11999990000", "loja@x.com", enderecoId);

        Assert.Equal("Loja X", empresa.Nome);
        Assert.Equal("12345678000199", empresa.CNPJ);
        Assert.Equal("loja@x.com", empresa.Email);
        Assert.Equal("11999990000", empresa.Telefone);
        Assert.Equal(enderecoId, empresa.EnderecoId);
    }

    [Fact(DisplayName = "Empresa: Mutações atualizam os campos")]
    public void EmpresaMutacoes_AtualizamCampos()
    {
        var empresa = new Empresa("A", "11111111111111", null, "a@a.com", Guid.NewGuid());

        empresa.AtualizarNome("Novo Nome");
        empresa.AtualizarCNPJ("22222222222222");
        empresa.AtualizarEmail("novo@email.com");
        empresa.AtualizarTelefone("33333333333");

        Assert.Equal("Novo Nome", empresa.Nome);
        Assert.Equal("22222222222222", empresa.CNPJ);
        Assert.Equal("novo@email.com", empresa.Email);
        Assert.Equal("33333333333", empresa.Telefone);
    }

    // ────────────── Usuario ──────────────

    [Fact(DisplayName = "Usuario: Construtor preenche campos")]
    public void UsuarioConstrutor_PreencheCampos()
    {
        var empresaId = Guid.NewGuid();
        var usuario = new Usuario(empresaId, "Ana", "ana@email.com", "SenhaHash123", CargoUsuario.Gerente);

        Assert.Equal("Ana", usuario.Nome);
        Assert.Equal("ana@email.com", usuario.Email);
        Assert.Equal("SenhaHash123", usuario.Senha);
        Assert.Equal(CargoUsuario.Gerente, usuario.Cargo);
        Assert.Equal(empresaId, usuario.EmpresaId);
    }

    [Fact(DisplayName = "Usuario: Mutações atualizam os campos")]
    public void UsuarioMutacoes_AtualizamCampos()
    {
        var usuario = new Usuario(Guid.NewGuid(), "A", "a@a.com", "Senha1!", CargoUsuario.Funcionario);

        usuario.AtualizarNome("Novo");
        usuario.AtualizarEmail("novo@email.com");
        usuario.AtualizarSenha("NovaSenha1!");
        usuario.AtualizarCargo(CargoUsuario.Administrador);

        Assert.Equal("Novo", usuario.Nome);
        Assert.Equal("novo@email.com", usuario.Email);
        Assert.Equal("NovaSenha1!", usuario.Senha);
        Assert.Equal(CargoUsuario.Administrador, usuario.Cargo);
    }

    // ────────────── Compra ──────────────

    [Fact(DisplayName = "Compra: Construtor preenche campos")]
    public void CompraConstrutor_PreencheCampos()
    {
        var clienteId = Guid.NewGuid();
        var empresaId = Guid.NewGuid();
        var compra = new Compra(150.50m, "Mantimentos", "NF-001", clienteId, empresaId);

        Assert.Equal(150.50m, compra.Valor);
        Assert.Equal("Mantimentos", compra.Descricao);
        Assert.Equal("NF-001", compra.NumeroNota);
        Assert.Equal(clienteId, compra.ClienteId);
        Assert.Equal(empresaId, compra.EmpresaId);
    }

    [Fact(DisplayName = "Compra: Mutações atualizam valores")]
    public void CompraMutacoes_AtualizamValores()
    {
        var compra = new Compra(100, "Desc", "NF-1", Guid.NewGuid(), Guid.NewGuid());
        compra.AtualizarValor(200);
        compra.AtualizarDescricao("Nova Desc");
        compra.AtualizarNumeroNota("NF-2");

        Assert.Equal(200, compra.Valor);
        Assert.Equal("Nova Desc", compra.Descricao);
        Assert.Equal("NF-2", compra.NumeroNota);
    }

    // ────────────── Pagamento ──────────────

    [Fact(DisplayName = "Pagamento: Construtor preenche campos")]
    public void PagamentoConstrutor_PreencheCampos()
    {
        var clienteId = Guid.NewGuid();
        var empresaId = Guid.NewGuid();
        var data = DateTime.UtcNow;
        var pagamento = new Pagamento(clienteId, empresaId, 250m, "Parcial", data);

        Assert.Equal(250m, pagamento.ValorPagamento);
        Assert.Equal("Parcial", pagamento.Observacao);
        Assert.Equal(data, pagamento.DataPagamento);
        Assert.Equal(clienteId, pagamento.ClienteId);
        Assert.Equal(empresaId, pagamento.EmpresaId);
    }

    // ────────────── Endereco ──────────────

    [Fact(DisplayName = "Endereco: Construtor preenche todos os campos")]
    public void EnderecoConstrutor_PreencheCampos()
    {
        var end = new Endereco("Brasil", "MG", "BH", "Centro", "Rua B", "200", "30130000");

        Assert.Equal("Brasil", end.Pais);
        Assert.Equal("MG", end.Estado);
        Assert.Equal("BH", end.Cidade);
        Assert.Equal("Centro", end.Bairro);
        Assert.Equal("Rua B", end.Rua);
        Assert.Equal("200", end.Numero);
        Assert.Equal("30130000", end.CEP);
    }

    [Fact(DisplayName = "Endereco: Mutações atualizam campos")]
    public void EnderecoMutacoes_AtualizamCampos()
    {
        var end = new Endereco("Brasil", "SP", "SP", "Centro", "Rua A", "1", "01000000");
        end.AtualizarPais("USA");
        end.AtualizarEstado("CA");
        end.AtualizarCidade("LA");
        end.AtualizarBairro("Downtown");
        end.AtualizarRua("Main St");
        end.AtualizarNumero("100");
        end.AtualizarCEP("90001");

        Assert.Equal("USA", end.Pais);
        Assert.Equal("CA", end.Estado);
        Assert.Equal("LA", end.Cidade);
        Assert.Equal("Downtown", end.Bairro);
        Assert.Equal("Main St", end.Rua);
        Assert.Equal("100", end.Numero);
        Assert.Equal("90001", end.CEP);
    }
}
