using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories
{
    public class EnderecoRepository : BaseRepository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(ConnectionContext context) : base(context)
        {
        }

        public async Task<Endereco?> BuscarPorCEP(string CEP)
        {
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.CEP == CEP);

            return endereco;
        }
    }
}
