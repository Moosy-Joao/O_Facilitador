using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;

namespace facilitador_api.Infrastructure.Repositories
{
    public class PagamentoRepository : BaseRepository<Pagamento>, IPagamentoRepository
    {
        public PagamentoRepository(ConnectionContext context) : base(context)
        {
        }

        public async Task<Pagamento?> BuscarPorCliente(string Cliente)
        {
            var pagamento = _context.Pagamentos.FirstOrDefault(e => e.Cliente == Cliente);

            if (pagamento == null)
            {
                throw new Exception("Pagamento não encontrado para o Cliente informado.");
            }

            return pagamento;
        }

        public async Task<Pagamento?> BuscarPorEmpresa(String Empresa)
        {
            var empresa = _context.Empresas.Find(Empresa);

            if (empresa == null)
            {
                throw new Exception("Pagamento não encontrado na empresa escolhida.");
            }

            return empresa;
        }

        public async Task<Pagamento?> BuscarPorData(DateTime PagamentoData)
        {
            var pagamento = _context.Pagamentos.Find(PagamentoData);

            if (pagamento == null)
            {
                throw new Exception("Pagamento não encontrado na data escolhida.");
            }

            return pagamento;
        }
    }
}
