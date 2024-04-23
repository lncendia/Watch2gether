namespace PJMS.AuthService.Web.Consent.ViewModels;

/// <summary>
/// Класс, представляющий модель представления для области видимости.
/// </summary>
public class ScopeViewModel
{
    /// <summary>
    /// Значение области видимости.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Отображаемое имя области видимости.
    /// </summary>
    public required string DisplayName { get; init; }

    /// <summary>
    /// Описание области видимости (необязательно).
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Подчеркивание области видимости.
    /// </summary>
    public required bool Emphasize { get; init; }

    /// <summary>
    /// Обязательность области видимости.
    /// </summary>
    public required bool Required { get; init; }

    /// <summary>
    /// Проверка области видимости.
    /// </summary>
    public required bool Checked { get; set; }
}