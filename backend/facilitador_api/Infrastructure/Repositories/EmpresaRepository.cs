using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly ConnectionContext _context;

        public EmpresaRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Atualizar(Empresa empresa)
        {
            try
            {
                var existingEmpresa = _context.Empresas
                    .Include(e => e.Endereco)
                    .FirstOrDefault(e => e.Id == empresa.Id);

                if (existingEmpresa == null)
                {
                    throw new Exception("Empresa não encontrada.");
                }

                existingEmpresa = empresa;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao atualizar a empresa.", ex);
            }
        }

        public Empresa? BuscarPorId(Guid id)
        {
            try
            {
                var empresa = _context.Empresas
                    .Include(e => e.Endereco)
                    .FirstOrDefault(e => e.Id == id);
                if (empresa == null)
                {
                    throw new Exception("Empresa não encontrada.");
                }
                return empresa;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao buscar a empresa por ID.", ex);
            }
        }

        public Empresa? BuscarPorNome(string nome)
        {
            try
            {
                var empresa = _context.Empresas
                    .Include(e => e.Endereco)
                    .FirstOrDefault(e => e.Nome.ToLower() == nome.ToLower());
                if (empresa == null)
                {
                    throw new Exception("Empresa não encontrada.");
                }
                return empresa;

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao buscar a empresa por nome.", ex);
            }
        }

        public void Cadastrar(Empresa empresa)
        {
            try
            {
                _context.Empresas.Add(empresa);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao cadastrar a empresa.", ex);
            }
        }

        public void Desativar(Guid id)
        {
            try
            {
                var empresa = _context.Empresas.Find(id);
                if (empresa == null)
                {
                    throw new Exception("Empresa não encontrada.");
                }

                if (empresa.Ativo == false)
                {
                    throw new Exception("Empresa já está desativada.");
                }

                empresa.Desativar();
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao desativar a empresa.", ex);
            }
        }
    }
}
