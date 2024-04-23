using Room.Infrastructure.Storage.DatabaseInitialization;
using Room.Infrastructure.Web.Rooms.Hubs;
using Room.Start.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("server.json", optional: false, reloadOnChange: true)
    .AddJsonFile("jwt.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Services.AddMemoryCache();

builder.Services.AddMassTransitServices(builder.Configuration);

// Добавление служб Mediator
builder.Services.AddMediatorServices();

builder.Services.AddJwtAuthorization(builder.Configuration);

// Добавление служб для хранилища
builder.Services.AddPersistenceServices(builder.Configuration);

// Добавление служб для работы с CORS
builder.Services.AddCorsServices();

builder.Services.AddSignalR();



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

app.MapHub<FilmRoomHub>("/filmRoom");

// Запуск приложения
await app.RunAsync();