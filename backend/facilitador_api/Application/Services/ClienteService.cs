using facilitador_api.Application.Interfaces;
using facilitador_api.Application.DTOs;
using facilitador_api.Domain.Interfaces;
using facilitador_api.Domain.Entities;
using System.Linq;

namespace facilitador_api.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public string Criar(ClienteDTO dto)
        {
            // 🔹 Validações
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return "Nome é obrigatório";

            if (!ValidarDocumento(dto.Documento))
                return "CPF/CNPJ inválido";

            if (!ValidarTelefone(dto.Telefone))
                return "Telefone inválido";

            // 🔹 Duplicidade
            var existente = _repository.ObterPorDocumento(dto.Documento);
            if (existente != null)
                return "Cliente já cadastrado";

            // 🔹 Criar entidade
            var cliente = new Cliente(dto.Nome, dto.Documento, dto.Telefone);

            _repository.Adicionar(cliente);

            return "Cliente cadastrado com sucesso";
        }

        private bool ValidarDocumento(string doc)
        {
            if (string.IsNullOrWhiteSpace(doc))
                return false;

            doc = new string(doc.Where(char.IsDigit).ToArray());

            return doc.Length == 11 || doc.Length == 14;
        }

        private bool ValidarTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return false;

            telefone = new string(telefone.Where(char.IsDigit).ToArray());

            return telefone.Length >= 10 && telefone.Length <= 11;
        }
    }
}