
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQWebAPI.Interface;
using RabbitMQWebAPI.Models;
using RabbitMQWebAPI.Services;

namespace RabbitMQWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IRabbitMQ _rabbitMQService;
        private readonly IBus _bus;

        public MessagesController(IRabbitMQ rabbitMQService, IBus bus)
        {
            _rabbitMQService = rabbitMQService;
            _bus = bus;
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishMessage([FromBody] Message message)
        {
      
            await _rabbitMQService.PublishMessageAsync(message);
            return Ok("Message published successfully.");
        }
    }
}
