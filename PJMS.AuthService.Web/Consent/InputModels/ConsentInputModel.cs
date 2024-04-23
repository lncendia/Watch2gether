using System.ComponentModel.DataAnnotations;

namespace PJMS.AuthService.Web.Consent.InputModels;

/// <summary>
/// Модель ввода согласия
/// </summary>
public class ConsentInputModel
{
    /// <summary>
    /// Согласованные области применения
    /// </summary>
    public List<string> ScopesConsented { get; set; } =  [];

    /// <summary>
    /// Помните о согласии
    /// </summary>
    [Display(Name = "Remember", ResourceType = typeof(Resources.Consent.InputModels.ConsentInputModel))]
    public bool RememberConsent { get; set; }

    /// <summary>
    /// URL-адрес возврата
    /// </summary>
    public string ReturnUrl { get; init; } = "/";

    /// <summary>
    /// Описание
    /// </summary>
    [Display(Name = "Description", Prompt = "DescriptionPrompt", ResourceType = typeof(Resources.Consent.InputModels.ConsentInputModel))]
    public string? Description { get; set; }
}