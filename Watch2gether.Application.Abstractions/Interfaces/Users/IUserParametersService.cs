using Watch2gether.Domain.Users;

namespace Watch2gether.Application.Abstractions.Interfaces.Users;

public interface IUserParametersService
{
    Task<User> GetAsync(string email);
    Task ChangeNameAsync(string email, string name);
    Task ChangeAvatarAsync(string email, Stream avatar);
}