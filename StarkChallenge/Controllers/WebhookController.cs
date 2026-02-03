using StarkChallenge.DTOs;
using Microsoft.AspNetCore.Mvc;
using StarkChallenge.Repositorys;
using EllipticCurve;
using StarkChallenge.Interfaces.IServices;

namespace StarkChallenge.Controllers
{
    [ApiController]
    [Route("[controller]/starkbank")]
    public class WebhookController : Controller
    {
        private readonly IConfiguration _configuration; 
        private readonly ITransferService _transferService;

        public WebhookController(ITransferService transferService, IConfiguration configuration)
        {
            _transferService = transferService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("invoiceReceptor")]
        public async Task<IActionResult> GetResponseInvoice([FromBody] ResponseInvoiceDTO payload)
        {
            await _transferService.ValidateTransferProcess(payload);

            return Ok();
        }
    }
}
