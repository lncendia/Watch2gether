using Overoom.Application.Abstractions.User.DTOs;
using Overoom.Application.Abstractions.User.Entities;

namespace Overoom.Application.Abstractions.User.Interfaces;

public interface IUserManager
{
    Task<List<UserShortDto>> FindAsync(SearchQuery query);
    Task<UserData> GetAuthenticationDataAsync(Guid userId);
    Task<UserDto> GetAsync(Guid userId);
    Task EditAsync(EditUserDto editData);
    Task ChangePasswordAsync(string email, string password);
}