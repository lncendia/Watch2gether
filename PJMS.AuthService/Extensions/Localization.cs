using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.Abstractions.Accessories;
using PJMS.AuthService.Web.Services;
using PJMS.AuthService.Web.Services.Abstractions;

namespace PJMS.AuthService.Extensions;

/// <summary>
/// Статический класс, представляющий методы для добавления сервисов локализации.
/// </summary>
public static class Localization
{
    /// <summary>
    /// Добавляет сервисы локализации в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddLocalizationServices(this IServiceCollection services)

    {
        // Добавляем сервис локализации областей
        services.AddSingleton<IScopeLocalizer, ScopeLocalizer>();
        
        // Добавляет службы, необходимые для локализации приложения.
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        // Регистрирует действие, используемое для настройки определенного типа
        // параметров. Примечание. Они запускаются перед всеми .
        services.Configure<RequestLocalizationOptions>(options =>
        {
            // поддерживаемые культуры
            var supportedCultures = new[]
            {
                // английский
                new CultureInfo(LocalizationExtensions.En),
                // русский
                new CultureInfo(LocalizationExtensions.Ru),
                // немецкий
                new CultureInfo(LocalizationExtensions.De),
                // Китайский
                new CultureInfo(LocalizationExtensions.Zh),
                // Испанский
                new CultureInfo(LocalizationExtensions.Es),
                // Португальский
                new CultureInfo(LocalizationExtensions.Pt),
                // Японский
                new CultureInfo(LocalizationExtensions.Ja),
                // Турецкий
                new CultureInfo(LocalizationExtensions.Tr),
                // Французский
                new CultureInfo(LocalizationExtensions.Fr),
                // Итальянский
                new CultureInfo(LocalizationExtensions.It),
                // Литовский
                new CultureInfo(LocalizationExtensions.Lt),
                // Украинский
                new CultureInfo(LocalizationExtensions.Uk),
                // Польский
                new CultureInfo(LocalizationExtensions.Pl),
                // Румынский
                new CultureInfo(LocalizationExtensions.Ro),
                // Нидерландский
                new CultureInfo(LocalizationExtensions.Nl),
                // Венгерский
                new CultureInfo(LocalizationExtensions.Hu),
                // Греческий
                new CultureInfo(LocalizationExtensions.El),
                // Чешский
                new CultureInfo(LocalizationExtensions.Cs),
                // Шведский
                new CultureInfo(LocalizationExtensions.Sv),
                // Болгарский
                new CultureInfo(LocalizationExtensions.Bg),
                // Финский
                new CultureInfo(LocalizationExtensions.Fi),
                // Сербский
                new CultureInfo(LocalizationExtensions.Sr),
                // Корейский
                new CultureInfo(LocalizationExtensions.Ko),
                // Азербайджанский
                new CultureInfo(LocalizationExtensions.Az),
                // Казахский
                new CultureInfo(LocalizationExtensions.Kk),
                // Казахский
                new CultureInfo(LocalizationExtensions.Be),
            };

            /*
             * Получает или задает культуру по умолчанию для использования в
             * запросах, когда поддерживаемая культура не может быть определена
             * одним из настроенных IRequestCultureProviders. По умолчанию
             * используются CurrentCulture и CurrentUICulture.
             */
            options.DefaultRequestCulture = new RequestCulture("en", "en");
            /*
             * Культуры, поддерживаемые приложением. RequestLocalizationMiddleware
             * установит текущую культуру запроса только для записи в этом списке.
             * По умолчанию используется CurrentCulture.
             */
            options.SupportedCultures = supportedCultures;
            /*
             * Культуры пользовательского интерфейса, поддерживаемые приложением.
             * RequestLocalizationMiddleware установит текущую культуру запроса
             * только для записи в этом списке. По умолчанию используется CurrentUICulture.
             */
            options.SupportedUICultures = supportedCultures;
        });
    }
}