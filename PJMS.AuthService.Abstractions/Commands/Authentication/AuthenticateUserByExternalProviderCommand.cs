using MediatR;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Commands.Authentication;

/// <summary>
/// Команда для аутентификации пользователя через внешний провайдер.
/// </summary>
public class AuthenticateUserByExternalProviderCommand : IRequest<AppUser>
{
    /// <summary>
    /// Получает или задает провайдер аутентификации.
    /// </summary>
    public required string LoginProvider { get; init; }

    /// <summary>
    /// Получает или задает ключ провайдера аутентификации.
    /// </summary>
    public required string ProviderKey { get; init; }
}