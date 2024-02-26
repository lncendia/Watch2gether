using Room.Start.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddMassTransitServices(builder.Configuration);

// Добавление служб Mediator
builder.Services.AddMediatorServices();

// Добавление служб для хранилища
builder.Services.AddPersistenceServices(builder.Configuration);

// Добавление служб для работы с CORS
builder.Services.AddCorsServices();



// Создание приложения на основе настроек builder
using var app = builder.Build();

// Использование политик Cors
app.UseCors("DefaultPolicy");

app.UseAuthentication();

app.UseAuthorization();

// Запуск приложения
app.Run();