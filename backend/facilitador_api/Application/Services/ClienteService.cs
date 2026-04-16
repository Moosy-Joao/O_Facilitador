using facilitador_api.Application.DTOs;
using facilitador_api.Application.Interfaces;
using facilitador_api.Domain.Entities;
using facilitador_api.Domain.Interfaces;

namespace facilitador_api.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public ClienteService(IClienteRepository repository, IEmpresaRepository empresaRepository, IEnderecoRepository enderecoRepository)
        {
            _clienteRepository = repository;
            _empresaRepository = empresaRepository;
            _enderecoRepository = enderecoRepository;
        }

        public string Atualizar(Guid id, ClienteDTO dto)
        {
            try
            {
                // Verificar se a empresa existe
                var empresa = _empresaRepository.BuscarPorId(dto.Empresa);
                if (empresa == null)
                {
                    return "Empresa não encontrada.";
                }

                // Verificar se o endereço existe
                var endereco = _enderecoRepository.BuscarPorId(dto.Endereco);
                if (endereco == null)
                {
                    return "Endereço não encontrado.";
                }

                var cliente = _clienteRepository.BuscarPorId(id);

                if (cliente == null)
                {
                    return "Cliente não encontrado.";
                }

                var clienteAtualizado = new Cliente(
                    empresaId: dto.Empresa,
                    nome: dto.Nome,
                    email: dto.Email,
                    documento: dto.Documento,
                    telefone: dto.Telefone,
                    enderecoId: dto.Endereco,
                    saldo: dto.Saldo,
                    limiteCredito: dto.LimiteCredito
                );

                _clienteRepository.Atualizar(clienteAtualizado);
                return "Cliente atualizado com sucesso.";
            }
            catch
            {
                return "Ocorreu um erro ao atualizar o cliente.";
            }
        }

        public ClienteDTO? BuscarPorDocumento(string documento)
        {
            try
            {
                var cliente = _clienteRepository.BuscarPorDocumento(documento);
                if (cliente == null)
                {
                    throw new Exception("Cliente não encontrado.");
                }

                var endereco = _enderecoRepository.BuscarPorId(cliente.EnderecoId);
                if (endereco == null)
                {
                    throw new Exception("Endereço do cliente não encontrado.");
                }

                var empresa = _empresaRepository.BuscarPorId(cliente.EmpresaId);
                if (empresa == null)
                {
                    throw new Exception("Empresa do cliente não encontrada.");
                }

                return new ClienteResponseDTO
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Documento = cliente.Documento,
                    Telefone = cliente.Telefone,
                    Saldo = cliente.Saldo,
                    LimiteCredito = cliente.LimiteCredito,
                    Endereco = new EnderecoResponseDTO
                    {
                        Id = endereco.Id,
                        Pais = endereco.Pais,
                        Cidade = endereco.Cidade,
                        Estado = endereco.Estado,
                        Bairro = endereco.Bairro,
                        Rua = endereco.Rua,
                        Numero = endereco.Numero,
                        CEP = endereco.CEP
                    },
                    Empresa = new EmpresaResponseDTO
                    {
                        Id = empresa.Id,
                        Nome = empresa.Nome,
                        CNPJ = empresa.CNPJ,
                        Ativo = empresa.Ativo,
                        CriadoEm = empresa.CriadoEm
                    },
                    Ativo = cliente.Ativo,
                    CriadoEm = cliente.CriadoEm
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao buscar o cliente por documento: {ex.Message}");
            }
        }

        public ClienteDTO? BuscarPorEmail(string email)
        {
            try
            {
                var cliente = _clienteRepository.BuscarPorEmail(email);
                if (cliente == null)
                {
                    throw new Exception("Cliente não encontrado.");
                }

                var endereco = _enderecoRepository.BuscarPorId(cliente.EnderecoId);
                if (endereco == null)
                {
                    throw new Exception("Endereço do cliente não encontrado.");
                }

                var empresa = _empresaRepository.BuscarPorId(cliente.EmpresaId);
                if (empresa == null)
                {
                    throw new Exception("Empresa do cliente não encontrada.");
                }


            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao buscar o cliente por email: {ex.Message}");
            }
        }

        public ClienteDTO? BuscarPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClienteDTO> BuscarPorNome(string nome)
        {
            throw new NotImplementedException();
        }

        public string Criar(ClienteDTO dto)
        {
            try
            {
                // Verificar se a empresa existe
                var empresa = _empresaRepository.BuscarPorId(dto.Empresa);
                if (empresa == null)
                {
                    return "Empresa não encontrada.";
                }

                // Verificar se o endereço existe
                var endereco = _enderecoRepository.BuscarPorId(dto.Endereco);
                if (endereco == null)
                {
                    return "Endereço não encontrado.";
                }

                var cliente = new Cliente(
                    nome: dto.Nome,
                    email: dto.Email,
                    documento: dto.Documento,
                    telefone: dto.Telefone,
                    saldo: dto.Saldo,
                    limiteCredito: dto.LimiteCredito,
                    enderecoId: dto.Endereco,
                    empresaId: dto.Empresa
                );

                _clienteRepository.Cadastrar(cliente);
                return "Cliente criado com sucesso.";
            }
            catch
            {
                return "Ocorreu um erro ao criar o cliente.";
            }
        }

        public string Desativar(Guid id)
        {
            try
            {
                var cliente = _clienteRepository.BuscarPorId(id);
                if (cliente == null)
                {
                    return "Cliente não encontrado.";
                }

                cliente.Desativar();

                _clienteRepository.Atualizar(cliente);
                return "Cliente desativado com sucesso.";

            }
            catch
            {
                return "Ocorreu um erro ao desativar o cliente.";
            }
        }
    }
}
