namespace Overoom.Application.Abstractions.Users.Interfaces;

public interface IUserProfileService
{
    Task ChangeNameAsync(string email, string name);
    Task ChangeAvatarAsync(string email, Stream avatar);
}