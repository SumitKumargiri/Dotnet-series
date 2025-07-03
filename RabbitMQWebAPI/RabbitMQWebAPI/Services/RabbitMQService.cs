using crudoperation.Model;
using crudoperation.utility;
using Dapper;
using MassTransit;
using RabbitMQWebAPI.Interface;
using RabbitMQWebAPI.Models;

namespace RabbitMQWebAPI.Services
{
    public class RabbitMQService : IRabbitMQ
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly DBGateway _DBGateway;

        public RabbitMQService(IPublishEndpoint publishEndpoint,string connection)
        {
            _publishEndpoint = publishEndpoint;
            _DBGateway = new DBGateway(connection);
        }

        public async Task<ResultModel<Object>> PublishMessageAsync(Message message)
        {

            //var myMessage = new Message { Content = message };
            //await _publishEndpoint.Publish(myMessage);
            ResultModel<Object> result = new ResultModel<Object>();
            try
            {
                string query = @"insert into rabbitmq (Content, CreatedAt) values (@Content,@CreatedAt);";

                var par = new DynamicParameters();
                par.Add("@Content", message.Content);
                par.Add("@CreatedAt", DateTime.UtcNow);

                var insertdata = await _DBGateway.ExecuteAsync(query, par);
                await _publishEndpoint.Publish(message);
                //Console.WriteLine($"Message Published: {message.Content}");

                if (insertdata != 0)
                {
                    result.Success = true;
                    result.Message = "Data insert Successfully";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Data insert failed";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
