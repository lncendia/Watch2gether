using Films.Infrastructure.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Infrastructure.Storage.DatabaseInitialization;

/// <summary>
/// Класс для инициализации начальных данных в базу данных
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// Инициализация начальных данных в базу данных
    /// </summary>
    /// <param name="scopeServiceProvider">Определяет механизм для извлечения объекта службы,
    /// т. е. объекта, обеспечивающего настраиваемую поддержку для других объектов.</param>
    public static async Task InitAsync(IServiceProvider scopeServiceProvider)
    {
        // Получаем контекст базы данных
        var context = scopeServiceProvider.GetRequiredService<ApplicationDbContext>();

        //обновляем базу данных
        await context.Database.MigrateAsync();
    }
}