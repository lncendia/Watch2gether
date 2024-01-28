using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда для удаления внешней аутентификации пользователя.
/// </summary>
public class RemoveUserExternalLoginCommand : IRequest<UserData>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required long Id { get; init; }

    /// <summary>
    /// Получает или задает провайдер внешней аутентификации.
    /// </summary>
    public required string Provider { get; init; }
}