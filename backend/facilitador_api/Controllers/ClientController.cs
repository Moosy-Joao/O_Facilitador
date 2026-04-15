using facilitador_api.Application.Services;
using facilitador_api.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.Controllers
{
    [ApiController]
    [Route("api/v1/client")]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public IActionResult RegisterClient([FromBody] CreateClientRequest request)
        {
            var result = _clientService.RegisterClient(request);

            if (result.StatusCode == 201)
            {
                return CreatedAtAction(nameof(RegisterClient), new
                {
                    id = result.Client!.Id,
                    result.Client.Name,
                    CpfCnpj = result.Client.CNPJ,
                    result.Client.Phone
                });
            }

            if (result.StatusCode == 409)
            {
                return Conflict(new { message = result.Error });
            }

            return BadRequest(new { message = result.Error });
        }
    }
}