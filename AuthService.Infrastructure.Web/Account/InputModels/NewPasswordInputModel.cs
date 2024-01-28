using System.ComponentModel.DataAnnotations;

namespace AuthService.Infrastructure.Web.Account.InputModels;

public class NewPasswordInputModel
{
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    [DataType(DataType.Password)]
    [Display(Name = "NewPassword",
        ResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    public string? NewPassword { get; init; }

    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    [DataType(DataType.Password)]
    [Display(Name = "NewPasswordConfirm",
        ResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    [Compare("NewPassword", ErrorMessageResourceName = "NewPasswordConfirmError",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    public string? PasswordConfirm { get; init; }

    [Required]
    [EmailAddress]
    public string? Email { get; init; }

    [Required]
    public string? Code { get; init; }
    
    public string ReturnUrl { get; init; } = "/";
}