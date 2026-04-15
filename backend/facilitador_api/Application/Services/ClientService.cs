using System.Text.RegularExpressions;
using facilitador_api.Model;
using facilitador_api.Model.IReposytories;
using facilitador_api.ViewModel;

namespace facilitador_api.Application.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public ServiceResult RegisterClient(CreateClientRequest request)
        {
            var normalizedName = request.Name.Trim();
            var normalizedDocument = NormalizeDigits(request.CpfCnpj);
            var normalizedPhone = NormalizeDigits(request.Phone);

            if (string.IsNullOrWhiteSpace(normalizedName))
            {
                return ServiceResult.BadRequest("Nome é obrigatório.");
            }

            if (!IsValidCpfOrCnpj(normalizedDocument))
            {
                return ServiceResult.BadRequest("CPF/CNPJ inválido. Informe um CPF com 11 dígitos ou CNPJ com 14 dígitos.");
            }

            if (!IsValidPhone(normalizedPhone))
            {
                return ServiceResult.BadRequest("Telefone inválido. Informe entre 10 e 11 dígitos.");
            }

            if (_clientRepository.ExistsByDocumentOrPhone(normalizedDocument, normalizedPhone))
            {
                return ServiceResult.Conflict("Já existe cliente cadastrado com o mesmo CPF/CNPJ ou telefone.");
            }

            var client = new Client(normalizedName, normalizedDocument, normalizedPhone);
            _clientRepository.AddClient(client);

            return ServiceResult.Created(client);
        }

        private static bool IsValidCpfOrCnpj(string cpfCnpj)
        {
            return Regex.IsMatch(cpfCnpj, @"^\d{11}$") || Regex.IsMatch(cpfCnpj, @"^\d{14}$");
        }

        private static bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10,11}$");
        }

        private static string NormalizeDigits(string value)
        {
            return Regex.Replace(value ?? string.Empty, @"\D", string.Empty);
        }
    }

    public class ServiceResult
    {
        public int StatusCode { get; private set; }
        public string? Error { get; private set; }
        public Client? Client { get; private set; }

        public static ServiceResult BadRequest(string message) => new() { StatusCode = 400, Error = message };
        public static ServiceResult Conflict(string message) => new() { StatusCode = 409, Error = message };
        public static ServiceResult Created(Client client) => new() { StatusCode = 201, Client = client };
    }
}