using Dllprocess.Interface;
using GrpcCrudService;
using GrpcCrudService1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

// Register your CRUD service implementation
builder.Services.AddScoped<ICrud, CrudServicegrpc>(); // Ensure you implement this interface

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CrudServicegrpc>(); // Register your gRPC service

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Use a gRPC client to communicate with this server.");
});

app.Run();
