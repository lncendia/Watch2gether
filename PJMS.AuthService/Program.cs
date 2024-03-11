using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.Data.DatabaseInitialization;
using PJMS.AuthService.Data.IdentityServer.HostExtensions;
using PJMS.AuthService.Extensions;

// Обработка значений даты и времени для PostgreSQL (для MsSql закомментировать)
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Создаем билдер приложения
var builder = WebApplication.CreateBuilder(args);

// Настраиваем конфигурацию приложения.
builder.Configuration

  // Установка базового пути для поиска конфигурационных файлов.
  .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))

  // Добавление конфигурационного файла appsettings.json.
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)

  // Добавление конфигурационного файла email.json.
  .AddJsonFile("email.json", optional: false, reloadOnChange: false)

  // Добавление конфигурационного файла oauth.json.
  .AddJsonFile("oauth.json", optional: false, reloadOnChange: false)

  // Добавление конфигурационного файла loggers.json.
  .AddJsonFile("loggers.json", optional: false, reloadOnChange: false);

// Добавляет службы для контроллеров в указанную коллекцию IServiceCollection.
builder.Services.AddControllersWithViews()

  // Добавляет в приложение локализацию аннотаций данных MVC.
  .AddDataAnnotationsLocalization()

  // компиляцию View при изменениях
  .AddRazorRuntimeCompilation()

  // Добавляет службы локализации представлений MVC в приложение.
  .AddViewLocalization();


// Добавляет службы совместного использования ресурсов из разных источников.
builder.Services.AddCorsServices();

// Добавляет службы аутентификации OAuth с использованием конфигурации.
builder.Services.AddOauth(builder.Configuration);

// Добавляет службы ASP.NET Identity.
builder.Services.AddAspIdentity(builder.Configuration, builder.Environment.WebRootPath);

// Добавляет службы Identity Server.
builder.Services.AddIdentityServerWithStores(builder.Configuration);

// Добавляет службы MassTransit.
builder.Services.AddMassTransitServices(builder.Configuration);

// Добавляет службы Swagger для документации API.
builder.Services.AddSwagger();

// Добавляет службы локализации для поддержки многоязычности в приложении
builder.Services.AddLocalizationServices();

// Добавляет медиатор и регистрирует обработчики
builder.Services.AddMediatorServices();

// Добавляет службы электронной почты с использованием конфигурации
builder.Services.AddEmail(builder.Configuration);

// Создаем объект приложения
await using var app = builder.Build();

// Создаем область для инициализации баз данных
using (var scope = app.Services.CreateScope())
{
  // Инициализация начальных данных в базу данных
  await DatabaseInitializer.InitAsync(scope.ServiceProvider);

  // Инициализация начальных данных в базу данных
  await IdentityServerInitializer.InitAsync(scope.ServiceProvider);
}

// Добавляет RequestLocalizationMiddleware для автоматической установки
// сведений о культуре для запросов на основе информации, предоставленной клиентом.
app.UseRequestLocalization();

// Добавляем хендлер исключений
app.UseExceptionHandler("/Home/Error");

// Включает статическое обслуживание файлов для текущего пути запроса
app.UseStaticFiles();

// Добавляет ПО промежуточного слоя EndpointRoutingMiddleware
// в указанный IApplicationBuilder.
app.UseRouting();

// Добавляет ПО промежуточного слоя CORS в конвейер веб-приложений,
// чтобы разрешить междоменные запросы.
app.UseCors("DefaultPolicy");

// Добавляет AuthenticationMiddleware в указанный IApplicationBuilder,
// что позволяет использовать возможности проверки подлинности.
app.UseAuthentication();

// Добавляет AuthorizationMiddleware в указанный IApplicationBuilder,
// что позволяет использовать возможности авторизации.
app.UseAuthorization();

// Добавляет IdentityServer в конвейер.
app.UseIdentityServer();

// Мапим маршруты к контроллерам с использованием маршрута по умолчанию
app.MapDefaultControllerRoute();

// Запускаем приложение
await app.RunAsync();