using crudoperation.Model;
using RabbitMQWebAPI.Models;

namespace RabbitMQWebAPI.Interface
{
    public interface IRabbitMQ
    {
        Task <ResultModel<Object>> PublishMessageAsync(Message message);
    }
}
