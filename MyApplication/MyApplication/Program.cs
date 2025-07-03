using CoreLayer.Interfaces;
using InfrastructureLayer.Repositories;
using InfrastructureLayer.utility;
using Microsoft.Extensions.DependencyInjection;
using MyApplicationApplicationLayer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the connection string
//var connectionString = builder.Configuration.GetConnectionString("ConnectionString1");

// Register the DBGateway and pass the connection string

string connectionString = builder.Configuration.GetConnectionString("ConnectionString1");

builder.Services.AddScoped<ICrudRepository>(provider => new CrudRepository(connectionString));
//builder.Services.AddScoped<ICrud>(provider => new CrudService());

// Register the repository and service layers
//builder.Services.AddScoped<ICrudRepository, CrudRepository>();
builder.Services.AddScoped<ICrud, CrudService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
