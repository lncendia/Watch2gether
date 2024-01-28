using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда для аутентификации пользователя через внешний провайдер.
/// </summary>
public class AuthenticateUserByExternalProviderCommand : IRequest<UserData>
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