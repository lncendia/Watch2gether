using Room.Infrastructure.Storage.DatabaseInitialization;
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
await using var app = builder.Build();

// Создаем область для инициализации баз данных
using (var scope = app.Services.CreateScope())
{
    // Инициализация начальных данных в базу данных
    await DatabaseInitializer.InitAsync(scope.ServiceProvider);
}

// Использование политик Cors
app.UseCors("DefaultPolicy");

app.UseAuthentication();

app.UseAuthorization();

// Запуск приложения
await app.RunAsync();