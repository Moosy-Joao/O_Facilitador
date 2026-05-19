using facilitador_domain.Domain.DTOs;
using facilitador_domain.Domain.Enums;

namespace facilitador_api.Domain.Entities
{
    public class Usuario : BaseModel
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public CargoUsuario Cargo { get; private set; }

        // relacionamento com Empresa
        public Guid EmpresaId { get; private set; }
        public Empresa Empresa { get; private set; }

        public Usuario() { }

        public Usuario(Guid empresaId, string nome, string email, string senha, CargoUsuario cargo)
        {
            EmpresaId = empresaId;
            Nome = nome;
            Email = email;
            Senha = senha;
            Cargo = cargo;

        }

        public Usuario(UsuarioCreateDTO dto, Guid empresaId)
        {
            EmpresaId = empresaId;
            Nome = dto.Nome;
            Email = dto.Email;
            Senha = dto.Senha;
            Cargo = dto.Cargo;
        }

        public void AtualizarEmpresa(Guid empresaId) => EmpresaId = empresaId;

        public void AtualizarNome(string nome) => Nome = nome;

        public void AtualizarEmail(string email) => Email = email;

        public void AtualizarSenha(string senha) => Senha = senha;

        public void AtualizarCargo(CargoUsuario cargo) => Cargo = cargo;

    }
}
