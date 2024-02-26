namespace PJMS.AuthService.Web.Settings.ViewModels;

/// <summary>
/// Модель представления для страницы настроек.
/// </summary>
public class SettingsViewModel
{
    /// <summary>
    /// Внешние поставщики
    /// </summary>
    public required IEnumerable<ExternalProvider> ExternalProviders { get; init; }

    /// <summary>
    /// Флаг, определяющий необходимо ли вводить старый пароль
    /// </summary>
    public required bool ShowOldPassword { get; init; }
    
    /// <summary>
    /// Флаг, определяющий, включена ли 2fa аутентификация
    /// </summary>
    public required bool TwoFactorEnabled { get; init; }
    
    /// <summary>
    /// Номер вкладки, которая должна быть раскрыта
    /// </summary>
    public required int ExpandElement { get; init; }
    
    /// <summary>
    /// Почта пользователя
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public required string UserName { get; init; }
    
    /// <summary>
    /// URL аватара
    /// </summary>
    public required Uri Thumbnail { get; init; }
    
    /// <summary>
    /// URL возврата
    /// </summary>
    public required string ReturnUrl { get; init; }
    
    /// <summary>
    /// Сообщение для пользователя
    /// </summary>
    public required string? Message { get; init; }
}