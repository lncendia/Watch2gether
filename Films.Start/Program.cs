using Films.Start.Extensions;
using Films.Start.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("movieApi.json", optional: false, reloadOnChange: true)
    .AddJsonFile("jwt.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Services.AddMemoryCache();

//builder.Services.AddHostedService<FilmLoadHostedService>();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddCorsServices();

builder.Services.AddJwtAuthorization(builder.Configuration);

// Регистрация Swagger генератора
builder.Services.AddSwaggerServices();

// Добавление служб Mediator
builder.Services.AddMediatorServices();

// Добавление служб для хранилища
builder.Services.AddPersistenceServices(builder.Configuration, builder.Environment.WebRootPath);

// Регистрация контроллеров с поддержкой сериализации JSON
builder.Services.AddControllers();

// Добавление служб для работы с CORS
builder.Services.AddCorsServices();



// Создание приложения на основе настроек builder
using var app = builder.Build();

// Добавляем мидлварь обработки ошибок
app.UseMiddleware<ExceptionMiddleware>();

// Использование Swagger для обслуживания документации по API
app.UseSwagger();

// Использование Swagger UI для предоставления интерактивной документации по API
app.UseSwaggerUI();

// Использование политик Cors
app.UseCors("DefaultPolicy");

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

// Маппим контроллеры
app.MapControllers();

// Запуск приложения
app.Run();