using AuthService.Application.Abstractions.Entities;

namespace AuthService.Infrastructure.Web.Services;

public interface IJwtService
{
    Task<string> GenerateJwt(UserData user);
}