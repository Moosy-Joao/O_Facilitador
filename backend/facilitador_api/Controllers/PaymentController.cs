using facilitador_api.Application.DTOs;
using facilitador_api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace facilitador_api.API.Controllers
{
    [ApiController]
    [Route("Payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CriarPagamento(PaymentDTO dto)
        {
            var resultado = _service.Criar(dto);

            if (resultado != "Pagamento cadastrado com sucesso")
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
}