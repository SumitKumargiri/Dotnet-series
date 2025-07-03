using MassTransit;
using RabbitMQWebAPI.Interface;
using RabbitMQWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//string connectionstring = builder.Configuration.GetConnectionString("ConnectionString1");

builder.Services.AddTransient<IRabbitMQ>(sp =>
{
    var publishEndpoint = sp.GetRequiredService<IPublishEndpoint>();
    string connection = builder.Configuration.GetConnectionString("ConnectionString1");
    return new RabbitMQService(publishEndpoint, connection);
});


//++++++++ RabbitMQ Register ++++++++++++++++
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});
builder.Logging.AddConsole();
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});


////builder.Services.AddScoped<RabbitMQService>();
//builder.Services.AddScoped<IRabbitMQ, RabbitMQService>(provider =>
//{
//    return new RabbitMQService(connectionstring);
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

