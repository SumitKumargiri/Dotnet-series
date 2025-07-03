// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using MassTransit;
using RabbitMQClient.Models;

class Program
{
    static async Task Main(string[] args)
    {
        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host("rabbitmq://localhost", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });

            cfg.ReceiveEndpoint("message_queue", ep =>
            {
                ep.Handler<Message>(context =>
                {
                    var message = context.Message;
                    Console.WriteLine($"Received Message: {message.Content}, CreatedAt: {message.CreatedAt}");
                    return Task.CompletedTask;
                });
            });
        });

        Console.WriteLine("Listening for messages...");
        await busControl.StartAsync();

        try
        {
            Console.WriteLine("Press any key to exit.");
            await Task.Run(() => Console.ReadKey());
        }
        finally
        {
            await busControl.StopAsync();
        }
    }
}
