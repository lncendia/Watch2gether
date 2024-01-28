using AuthService.Infrastructure.Web.Settings.InputModels;

namespace AuthService.Infrastructure.Web.Settings.ViewModels;

public class ChangeAvatarViewModel : ChangeAvatarInputModel
{
    public required Uri AvatarUrl { get; init; }
}