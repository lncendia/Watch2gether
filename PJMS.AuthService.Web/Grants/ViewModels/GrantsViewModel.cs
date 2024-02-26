namespace PJMS.AuthService.Web.Grants.ViewModels;

/// <summary>
/// Класс, представляющий модель представления разрешений.
/// </summary>
public class GrantsViewModel
{
    /// <summary>
    /// Список моделей представления разрешений.
    /// </summary>
    public required IEnumerable<GrantViewModel> Grants { get; init; }
}