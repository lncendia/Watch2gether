namespace PJMS.AuthService.Web.Grants.ViewModels;

/// <summary>
/// Класс, представляющий модель представления разрешения.
/// </summary>
public class GrantViewModel
{
    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    public required string ClientId { get; init; }

    /// <summary>
    /// Название клиента.
    /// </summary>
    public required string ClientName { get; init; }

    /// <summary>
    /// URL-адрес клиента.
    /// </summary>
    public string? ClientUrl { get; init; }

    /// <summary>
    /// URL-адрес логотипа клиента.
    /// </summary>
    public string? ClientLogoUrl { get; init; }

    /// <summary>
    /// Описание разрешения.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Дата создания разрешения.
    /// </summary>
    public required DateTime Created { get; init; }

    /// <summary>
    /// Дата истечения срока разрешения.
    /// </summary>
    public DateTime? Expires { get; init; }

    /// <summary>
    /// Список имен ресурсов идентичности, связанных с разрешением.
    /// </summary>
    public required IEnumerable<string> IdentityGrantNames { get; init; }

    /// <summary>
    /// Список имен ресурсов API, связанных с разрешением.
    /// </summary>
    public required IEnumerable<string> ApiGrantNames { get; init; }
}