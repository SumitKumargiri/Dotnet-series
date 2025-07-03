using JobSeekingApplication.Interface;
using JobSeekingApplication.Services;
using JobSeekingApplication.utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

string connection = builder.Configuration.GetConnectionString("ConnectionString1");
builder.Services.AddSingleton<DBGateway>(provider =>new DBGateway(connection));
builder.Services.AddScoped<IAuth>(provider =>new AuthService(connection, builder.Configuration, provider.GetRequiredService<IDistributedCache>()));

builder.Services.AddScoped<IEmployee>(provider =>
{
    var dbGateway = provider.GetRequiredService<DBGateway>();
    return new EmployeeService(dbGateway, connection, builder.Configuration);
});

builder.Services.AddScoped<Ijobseeker>(provider =>
{
    var dbGateway = provider.GetRequiredService<DBGateway>();
    return new JobSeekerService(dbGateway);
});
builder.Services.AddScoped<IAdmin>(provider =>
{
    var dbGateway = provider.GetRequiredService<DBGateway>();
    return new AdminService(dbGateway);
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

//++++++++++++++++++++ Redis Configuration ++++++++++++++++++++++++++++++++
var redisConnectionString = builder.Configuration["Redis:ConnectionString"];  
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "JobSeekingApp_";
});


// ++++++++++++++++++++++++ JWT Authentication Configuration++++++++++++++++++++++++++++++
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


//+++++++++++++++++ Authentication Process Create the Lock+++++++++++++++++++++++++++

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
