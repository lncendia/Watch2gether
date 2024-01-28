using System.ComponentModel.DataAnnotations;

namespace AuthService.Infrastructure.Web.Settings.InputModels;

public class ChangeNameInputModel
{
    /// <summary>
    /// Логин (имя) пользователя
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangeNameInputModel))]
    [RegularExpression("^[A-Za-zА-Яа-яЁё0-9.@\\-_+ ]+$", ErrorMessageResourceName = "ValidUsername",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangeNameInputModel))]
    [Display(Name = "Username",
        ResourceType = typeof(Resources.Settings.InputModels.ChangeNameInputModel))]
    [MaxLength(20, ErrorMessageResourceName = "MaxLength",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangeNameInputModel))]
    [MinLength(3, ErrorMessageResourceName = "MinLength",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangeNameInputModel))]
    public string? Username { get; init; }
    
    public string ReturnUrl { get; init; } = "/";
    
}