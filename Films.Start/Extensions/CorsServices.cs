namespace Films.Start.Extensions;

/// <summary>
/// Класс устанавливающий CORS конфигурацию
/// </summary>
public static class CorsServices
{
    /// <summary>
    /// Устанавливает CORS конфигурацию
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    public static void AddCorsServices(this IServiceCollection services)
    {
        // Добавление настройки CORS
        services.AddCors(options =>
        {
            // Добавление политики CORS с именем "DefaultPolicy"
            options.AddPolicy("DefaultPolicy", builder =>
            {
                // Разрешение доступа с любого источника
                builder.AllowAnyOrigin();

                // Разрешение доступа к любым заголовкам
                builder.AllowAnyHeader();

                // Разрешение доступа к любым HTTP-методам
                builder.AllowAnyMethod();
            });
        });
    }
}