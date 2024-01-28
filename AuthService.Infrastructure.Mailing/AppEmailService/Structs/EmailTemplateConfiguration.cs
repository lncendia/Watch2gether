namespace AuthService.Infrastructure.Mailing.AppEmailService.Structs;

/// <summary>
/// Конфигурация шаблона электронного письма.
/// </summary>
public class EmailTemplateConfiguration
{
    /// <summary>
    /// Название компании.
    /// </summary>
    public required string CompanyName { get; init; }

    /// <summary>
    /// Ссылка на логотип компании.
    /// </summary>
    public required string LogoLink { get; init; }

    /// <summary>
    /// Ссылка на политику конфиденциальности.
    /// </summary>
    public required string PrivatePolicyLink { get; init; }

    /// <summary>
    /// Ссылка на главную страницу.
    /// </summary>
    public required string HomePageLink { get; init; }

    /// <summary>
    /// Ссылка на вторичный логотип.
    /// </summary>
    public required string SideLogoLink { get; init; }
}