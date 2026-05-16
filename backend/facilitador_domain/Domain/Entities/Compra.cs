using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Domain.Entities
{
    public class Compra : BaseModel
    {
        public decimal Valor { get; private set; }
        public string Descricao { get; private set; }
        public string NumeroNota { get; private set; }

        // Relacionamento com Cliente e Empresa
        public Guid ClienteId { get; private set; }
        public Cliente Cliente { get; private set; }

        public Guid EmpresaId { get; private set; }
        public Empresa Empresa { get; private set; }

        public Compra() { }

        public Compra(decimal valor, string descricao, string numeroNota, Guid clienteId, Guid empresaId)
        {
            Valor = valor;
            Descricao = descricao;
            NumeroNota = numeroNota;
            ClienteId = clienteId;
            EmpresaId = empresaId;
        }

        //public Compra(CompraCreateDTO dto, Guid clienteId, Guid empresaId)
        //{
        //    Valor = dto.Valor;
        //    Descricao = dto.Descricao;
        //    NumeroNota = dto.NumeroNota;
        //    ClienteId = clienteId;
        //    EmpresaId = empresaId;
        //}

        public void AtualizarValor(decimal novoValor) => Valor = novoValor;

        public void AtualizarDescricao(string novaDescricao) => Descricao = novaDescricao;

        public void AtualizarNumeroNota(string novoNumeroNota) => NumeroNota = novoNumeroNota;

        public void AtualizarCliente(Guid novoClienteId) => ClienteId = novoClienteId;

        public void AtualizarEmpresa(Guid novaEmpresaId) => EmpresaId = novaEmpresaId;
    }
}
