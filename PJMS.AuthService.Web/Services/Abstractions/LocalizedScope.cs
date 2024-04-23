namespace PJMS.AuthService.Web.Services.Abstractions;

/// <summary>
/// Определяет локализованное разрешение.
/// </summary>
public class LocalizedScope
{
    /// <summary>
    /// Получаем или устанавливаем отображаемое имя разрешения.
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Получаем или устанавливаем описание разрешения.
    /// </summary>
    public string? Description { get; init; }
}