using System.ComponentModel.DataAnnotations;

namespace AuthService.Infrastructure.Web.Account.InputModels;

public class RecoverPasswordInputModel
{
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.ResetPasswordInputModel))]
    [EmailAddress(ErrorMessageResourceName = "ValidEmail",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.ResetPasswordInputModel))]
    [Display(Name = "Email",
        ResourceType = typeof(Resources.Account.InputModels.ResetPasswordInputModel))]
    public string? Email { get; init; }

    public string ReturnUrl { get; init; } = "/";
}