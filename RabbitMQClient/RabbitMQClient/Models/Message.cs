using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQClient.Models
{
    public class Message
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
