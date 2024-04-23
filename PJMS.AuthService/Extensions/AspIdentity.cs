using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.Abstractions.AppThumbnailStore;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Data.DbContexts;
using PJMS.AuthService.Data.Services;
using PJMS.AuthService.Services.Validators;

namespace PJMS.AuthService.Extensions;

/// <summary>
/// Статический класс, представляющий методы для добавления ASP.NET Identity.
/// </summary>
public static class AspIdentity
{
    /// <summary>
    /// Добавляет ASP.NET Identity в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="rootPath">Путь к файлам приложения</param>
    public static void AddAspIdentity(this IServiceCollection services, IConfiguration configuration,
        string rootPath)
    {
        //Получаем из конфигурации строку подключения
        var aspIdentityDb = configuration.GetRequiredValue<string>("ConnectionStrings:AspIdentityDb");

        //Регистрирует данный контекст как службу в коллекции IServiceCollection.
        services.AddDbContext<ApplicationDbContext>(config =>
        {
            //указываем провайдера базы данных со строкой подключения
            config.UseNpgsql(aspIdentityDb);
        });

        // Добавляет валидатор для пользователя.
        services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();
        
        // Добавляет валидатор для пароля.
        services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();
        
        /* Добавляет и настраивает идентификационную систему для
         указанных пользователей и типов ролей.
         Устанавливает опции блокировки учетных записей. */
        services.AddIdentity<AppUser, AppRole>(opt =>
            {
                // Разрешает применение механизма блокировки для новых пользователей.
                opt.Lockout.AllowedForNewUsers = true;

                // Задает временной интервал блокировки по умолчанию в 15 минут
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                // Устанавливает максимальное количество неудачных попыток входа перед блокировкой
                opt.Lockout.MaxFailedAccessAttempts = 10;
            })

            //Добавляет реализацию Entity Framework хранилищ сведений об удостоверениях.
            .AddEntityFrameworkStores<ApplicationDbContext>()

            // Добавляет поставщиков токенов по умолчанию, используемых для
            // создания токенов для сброса паролей, операций изменения
            // электронной почты и номера телефона, а также для создания токенов
            // двухфакторной аутентификации.
            .AddDefaultTokenProviders();
        
        //Добавляет службы политики авторизации в указанную коллекцию IServiceCollection.
        services.AddAuthorization();
        

        // Получаем из конфигурации путь к сохранению миниатюр
        var thumbnailPath = configuration.GetRequiredValue<string>("Thumbnails");

        // Регистрация UserThumbnailStore как синглтон-сервиса с передачей rootPath и thumbnailPath в конструктор.
        services.AddSingleton<IThumbnailStore, UserThumbnailStore>(_ =>
            new UserThumbnailStore(rootPath, thumbnailPath));
    }
}