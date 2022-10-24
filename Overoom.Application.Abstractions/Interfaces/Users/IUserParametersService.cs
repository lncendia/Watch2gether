using Overoom.Domain.Users;

namespace Overoom.Application.Abstractions.Interfaces.Users;

public interface IUserParametersService
{
    Task<User> GetAsync(string email);
    Task ChangeNameAsync(string email, string name);
    Task ChangeAvatarAsync(string email, Stream avatar);
}