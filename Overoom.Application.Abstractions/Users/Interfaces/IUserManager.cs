using Overoom.Application.Abstractions.Users.DTOs;
using Overoom.Application.Abstractions.Users.Entities;

namespace Overoom.Application.Abstractions.Users.Interfaces;

public interface IUserManager
{
    Task<List<UserShortDto>> FindAsync(SearchQuery query);
    Task<UserData> GetAuthenticationDataAsync(Guid userId);
    Task<UserDto> GetAsync(Guid userId);
    Task EditAsync(EditUserDto editData);
    Task ChangePasswordAsync(string email, string password);
}