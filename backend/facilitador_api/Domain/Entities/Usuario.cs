namespace facilitador_api.Domain.Entities
{
    public class Usuario : BaseModel
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public string Cargo { get; private set; }
        public string Imagem { get; private set; }

        // relacionamento com Empresa
        public Guid EmpresaId { get; private set; }
        public Empresa Empresa { get; private set; }

        public Usuario() { }

        public Usuario(Guid empresaId, string nome, string email, string senha, string cargo, string imagem)
        {
            EmpresaId = empresaId;
            Nome = nome;
            Email = email;
            Senha = senha;
            Cargo = cargo;
            Imagem = imagem;
        }

        //public Usuario(UsuarioCreateDTO dto, Guid empresaId)
        //{
        //    EmpresaId = empresaId;
        //    Nome = dto.Nome;
        //    Email = dto.Email;
        //    Senha = dto.Senha;
        //    Cargo = dto.Cargo;
        //    Imagem = dto.Imagem;
        //}

        public void AtualizarEmpresa(Guid empresaId) => EmpresaId = empresaId;

        public void AtualizarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return;
            Nome = nome;
        }

        public void AtualizarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;
            Email = email;
        }

        public void AtualizarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha)) return;
            Senha = senha;
        }

        public void AtualizarCargo(string cargo)
        {
            if (string.IsNullOrWhiteSpace(cargo)) return;
            Cargo = cargo;
        }

        public void AtualizarImagem(string imagem)
        {
            if (string.IsNullOrWhiteSpace(imagem)) return;
            Imagem = imagem;
        }
    }
}
