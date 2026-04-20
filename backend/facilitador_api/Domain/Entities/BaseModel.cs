namespace facilitador_api.Domain.Entities
{
    public abstract class BaseModel
    {
        public Guid Id { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime ModificadoEm { get; private set; }

        public BaseModel()
        {
            Ativo = true;
            CriadoEm = DateTime.UtcNow;
            ModificadoEm = DateTime.UtcNow;
        }

        public void Ativar()
        {
            Ativo = true;
            ModificadoEm = DateTime.UtcNow;
        }

        public void Desativar()
        {
            Ativo = false;
            ModificadoEm = DateTime.UtcNow;
        }

        public void AtualizarModificadoEm(DateTime dataHora)
        {
            ModificadoEm = dataHora;
        }
    }
}
