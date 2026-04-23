using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;

namespace facilitador_api.Infrastructure.Repositories
{
    public class EnderecoRepository : BaseRepository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(ConnectionContext context) : base(context)
        {
        }

        public async Task<Endereco?> BuscarPorCEP(string CEP)
        {
            var endereco = _context.Enderecos.FirstOrDefault(e => e.CEP == CEP);

            if (endereco == null)
            {
                throw new Exception("Endereço não encontrado para o CEP fornecido.");
            }

            return endereco;
        }

        public async Task<Endereco?> BuscarPorId(Guid id)
        {
            var endereco = _context.Enderecos.Find(id);

            if (endereco == null)
            {
                throw new Exception("Endereço não encontrado para o ID fornecido.");
            }

            return endereco;
        }
    }
}
