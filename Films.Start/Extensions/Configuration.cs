using Films.Start.Exceptions;

namespace Films.Start.Extensions;

/// <summary>
/// Статический класс, предоставляющий метод расширения для получения обязательного значения из IConfiguration.
/// </summary>
public static class Configuration
{
    /// <summary>
    /// Получает обязательное значение из IConfiguration по указанному ключу.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="configuration">Экземпляр IConfiguration.</param>
    /// <param name="key">Ключ для получения значения.</param>
    /// <returns>Обязательное значение.</returns>
    /// <exception cref="ConfigurationException">Вызывается, если значение не найдено или не может быть приведено к указанному типу.</exception>
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
    {
        // Возвращаем полученное значение или вызываем исключение, если оно не получено
        return configuration.GetValue<T?>(key) ?? throw new ConfigurationException(key);
    }
}