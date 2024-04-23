using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.Abstractions.AppThumbnailStore;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Data;
using PJMS.AuthService.Data.DbContexts;
using PJMS.AuthService.Data.IdentityServer.HostExtensions;
using PJMS.AuthService.Data.Services;
using PJMS.AuthService.Services.Validators;

namespace PJMS.AuthService.Extensions;

/// <summary>
/// Статический класс, представляющий методы для добавления Identity Server.
/// </summary>
public static class IdentityServer
{
    /// <summary>
    /// Добавляет Identity Server в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static void AddIdentityServerWithStores(this IServiceCollection services, IConfiguration configuration)
    {
        //Получаем из конфигурации строку подключения
        var identityServerDb = configuration.GetRequiredValue<string>("ConnectionStrings:IdentityServerDb");


        //Добавляет IdentityServer
        services.AddIdentityServer()

            // Интегрируем Identity Server с ASP.NET Identity, используя тип пользователя AppUser.
            .AddAspNetIdentity<AppUser>()

            // Настраиваем хранилище конфигурации для Identity Server, используя PostgreSQL базу данных.
            .AddConfigurationStore(options =>
            {
                // Настраиваем базу данных и указываем, что связанные коллекции в запросе должны быть разбиты на несколько запросов sql
                options.ConfigureDbContext = b => b.UseNpgsql(identityServerDb,
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            })

            // Настраиваем операционное хранилище для Identity Server, используя PostgreSQL базу данных.
            .AddOperationalStore(options =>
            {
                // Указываем, что хотим отчищать истекшие гранты и коды устройств
                options.EnableTokenCleanup = true;
                
                // Указываем интервал отчистки в 12 часов
                options.TokenCleanupInterval = 3600 * 12;
                
                // Настраиваем базу данных
                options.ConfigureDbContext = b => b.UseNpgsql(identityServerDb);
            })

            // Добавляем разработческий ключ подписи для использования в среде разработки.
            .AddDeveloperSigningCredential(); //todo: убрать в проде


        // Настраиваем куки для приложения
        services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
        {
            // Устанавливаем время истечения срока действия куки на 30 дней.
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
        });

        // Настраиваем куки для схемы запоминания двухфакторной аутентификации
        services.Configure<CookieAuthenticationOptions>(IdentityConstants.TwoFactorRememberMeScheme, options =>
        {
            // Устанавливаем время истечения срока действия куки на 30 дней.
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
        });

        // Настраиваем валидатор для куки
        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            // Устанавливаем интервал проверки валидации SecurityStamp на 15 минут.
            options.ValidationInterval = TimeSpan.FromMinutes(15);
        });
    }
}