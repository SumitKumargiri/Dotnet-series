using Microsoft.AspNetCore.Mvc;
using PhonePePaymentAPI.Services;
using System.Threading.Tasks;

namespace PhonePePaymentAPI.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly PhonePeService _phonePeService;

        public PaymentController(PhonePeService phonePeService)
        {
            _phonePeService = phonePeService;
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequest request)
        {
            if (request == null || request.Amount <= 0)
                return BadRequest(new { success = false, message = "Invalid amount." });

            var response = await _phonePeService.InitiatePayment(request.Amount);

            return Ok(new { success = true, response });
        }


        [HttpGet("status/{transactionId}")]
        public async Task<IActionResult> GetTransactionStatus(Guid transactionId)
        {
            var transaction = await _phonePeService.GetTransactionStatus(transactionId);

            if (transaction == null)
                return NotFound(new { success = false, message = "Transaction not found." });

            return Ok(new
            {
                success = true,
                transactionId = transaction.TransactionId,
                status = transaction.Status,
                amount = transaction.Amount,
                paymentMethod = transaction.PaymentMethod,
                createdAt = transaction.CreatedAt
            });
        }
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
    }
}
