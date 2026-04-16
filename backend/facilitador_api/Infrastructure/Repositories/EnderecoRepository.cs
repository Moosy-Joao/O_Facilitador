using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;

namespace facilitador_api.Infrastructure.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly ConnectionContext _context;

        public EnderecoRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Atualizar(Endereco endereco)
        {
            try
            {
                var enderecoExistente = _context.Enderecos.Find(endereco.Id);
                if (enderecoExistente == null)
                {
                    throw new Exception("Endereço não encontrado.");
                }

                enderecoExistente = endereco;

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o endereço: " + ex.Message);
            }
        }

        public Endereco BuscarPorCEP(string CEP)
        {
            try
            {
                var endereco = _context.Enderecos.FirstOrDefault(e => e.CEP == CEP);
                if (endereco == null)
                {
                    throw new Exception("Endereço não encontrado para o CEP fornecido.");
                }
                return endereco;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o endereço por CEP: " + ex.Message);
            }
        }

        public Endereco? BuscarPorId(Guid id)
        {
            try
            {
                var endereco = _context.Enderecos.Find(id);
                if (endereco == null)
                {
                    throw new Exception("Endereço não encontrado para o ID fornecido.");
                }
                return endereco;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o endereço por ID: " + ex.Message);
            }
        }

        public void Cadastrar(Endereco endereco)
        {
            try
            {
                _context.Enderecos.Add(endereco);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar o endereço: " + ex.Message);
            }
        }

        public void Desativar(Guid id)
        {
            try
            {
                var endereco = _context.Enderecos.Find(id);
                if (endereco == null)
                {
                    throw new Exception("Endereço não encontrado para o ID fornecido.");
                }

                if (endereco.Ativo == false)
                {
                    throw new Exception("Endereço já está desativado.");
                }

                endereco.Desativar();
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desativar o endereço: " + ex.Message);
            }
        }
    }
}
