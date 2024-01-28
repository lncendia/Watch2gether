using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace AuthService.Start.Extensions;

/// <summary>
/// Статический класс, представляющий методы для добавления сервисов локализации.
/// </summary>
public static class LocalizationServices
{
    /// <summary>
    /// Добавляет сервисы локализации в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddLocalizationServices(this IServiceCollection services)
    {
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
                new CultureInfo("en"),
                // русский
                new CultureInfo("ru"),
                // немецкий
                new CultureInfo("de"),
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