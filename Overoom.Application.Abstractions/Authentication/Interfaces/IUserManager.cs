using Overoom.Application.Abstractions.Authentication.DTOs;
using Overoom.Application.Abstractions.Authentication.Entities;

namespace Overoom.Application.Abstractions.Authentication.Interfaces;

public interface IUserManager
{
    Task<List<UserDto>> FindAsync(SearchQuery query);
    Task<UserData> GetAuthenticationDataAsync(Guid userId);
    Task<UserDto> GetAsync(Guid userId);
    Task EditAsync(EditUserDto editData);
    Task ChangePasswordAsync(string email, string password);
}