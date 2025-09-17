using Microsoft.Extensions.Options;
using OMS.Domain.Interfaces;
using OMS.Infrastructure.Repositories;
using OMS.Infrastructure.Services;
using OMS.Infrastructure.Services.Customers;
using OMS.Infrastructure.Services.Evidences;
using Polly;
using Polly.Retry;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//.AddJsonOptions(opts => {
//    opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // default ya lo hace
//    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//});

// Servicios de Infraestructura Singletons
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<IEventHistoryRepository, InMemoryEventHistoryRepository>();
builder.Services.AddSingleton<IFailedEvidenceRepository, InMemoryFailedEvidenceRepository>();

// Servicios aplicando Patrón Factory
builder.Services.AddSingleton<CustomerFalabellaNotificationService>();
builder.Services.AddSingleton<CustomerSodimacNotificationService>();
builder.Services.AddSingleton<CustomerRipleyNotificationService>();
builder.Services.AddSingleton<INotificationService, NotificationRouter>();
// Servicios de Almacenamiento de Evidencias
builder.Services.AddHttpClient<IEvidenceService, LocalFileEvidenceService>();


// Servicios de Aplicación o Lógica del Negocio
builder.Services.AddScoped<EventProcessor>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
