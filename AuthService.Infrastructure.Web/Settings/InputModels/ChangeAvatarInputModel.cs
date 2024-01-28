using System.ComponentModel.DataAnnotations;

namespace AuthService.Infrastructure.Web.Settings.InputModels;

public class ChangeAvatarInputModel
{
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangeAvatarInputModel))]
    public IFormFile? File { get; init; }

    public string ReturnUrl { get; init; } = "/";
}