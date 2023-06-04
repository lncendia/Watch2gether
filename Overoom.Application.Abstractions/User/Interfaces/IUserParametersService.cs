namespace Overoom.Application.Abstractions.User.Interfaces;

public interface IUserParametersService
{
    Task<Domain.User.Entities.User> GetAsync(string email);
    Task ChangeNameAsync(string email, string name);
    Task ChangeAvatarAsync(string email, Stream avatar);
}