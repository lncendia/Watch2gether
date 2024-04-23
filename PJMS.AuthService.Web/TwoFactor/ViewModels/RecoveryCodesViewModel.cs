namespace PJMS.AuthService.Web.TwoFactor.ViewModels;

/// <summary>
/// Модель представления кодов восстановления.
/// </summary>
public class RecoveryCodesViewModel
{
    /// <summary>
    /// Список кодов восстановления.
    /// </summary>
    public required IEnumerable<string> RecoveryCodes { get; init; }
}