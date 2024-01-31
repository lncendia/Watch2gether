using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда на закрытие остальных сессий пользователя
/// </summary>
public class CloseOtherUserSessionsCommand : IRequest<UserData>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required long Id { get; init; }
}