using MediatR;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда для проверки электронной почты пользователя.
/// </summary>
public class VerifyUserEmailCommand : IRequest
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required long Id { get; init; }

    /// <summary>
    /// Получает или задает код проверки.
    /// </summary>
    public required string Code { get; init; }
}