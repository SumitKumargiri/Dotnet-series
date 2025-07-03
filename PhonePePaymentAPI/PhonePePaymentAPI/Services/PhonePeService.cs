using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PhonePePaymentAPI.Services
{
    public class PhonePeService
    {
        private const string MerchantId = "PGTESTPAYUAT86";
        private const string SaltKey = "96434309-7796-489d-8924-ab56988a6076";
        private const int KeyIndex = 1;
        private const string PhonePeUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/pg/v1/pay";

        private readonly ApplicationDbContext _context;

        public PhonePeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> InitiatePayment(decimal amount)
        {
            string transactionId = Guid.NewGuid().ToString(); // Unique transaction ID
            var userId = Guid.NewGuid(); // Auto-generate
            var recipientId = Guid.NewGuid(); // Auto-generate

            var requestData = new
            {
                merchantId = MerchantId,
                merchantTransactionId = transactionId,
                amount = (int)(amount * 100), // Convert rupees to paise
                redirectUrl = $"http://localhost:3000/api/status?id={transactionId}",
                redirectMode = "POST",
                callbackUrl = $"http://localhost:3000/api/status?id={transactionId}",
                paymentInstrument = new { type = "PAY_PAGE" }
            };

            string payload = JsonConvert.SerializeObject(requestData);
            string payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));

            string checksumInput = payloadBase64 + "/pg/v1/pay" + SaltKey;
            string sha256Checksum = GenerateSHA256(checksumInput);
            string xVerify = $"{sha256Checksum}###{KeyIndex}";

            var client = new RestClient(PhonePeUrl);
            var request = new RestRequest("", Method.Post);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-VERIFY", xVerify);
            request.AddJsonBody(new { request = payloadBase64 });

            var response = await client.ExecuteAsync(request);

            // ✅ Save transaction in PostgreSQL
            var newTransaction = new PhonePeTransaction
            {
                TransactionId = Guid.Parse(transactionId),
                UserId = userId,
                RecipientId = recipientId,
                Amount = amount,
                TransactionType = "SEND",
                Status = "PENDING",
                PaymentMethod = "UPI",
                ReferenceId = null
            };

            _context.PhonePeTransactions.Add(newTransaction);
            await _context.SaveChangesAsync();

            return response.Content;
        }

        private static string GenerateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public async Task<PhonePeTransaction?> GetTransactionStatus(Guid transactionId)
        {
            return await _context.PhonePeTransactions
                                 .Where(t => t.TransactionId == transactionId)
                                 .FirstOrDefaultAsync();
        }
    }
}
