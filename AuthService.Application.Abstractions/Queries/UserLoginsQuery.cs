using MediatR;

namespace AuthService.Application.Abstractions.Queries;

/// <summary>
/// Запрос для получения списка внешних аутентификаций пользователя.
/// </summary>
public class UserLoginsQuery : IRequest<IReadOnlyCollection<string>>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required long Id { get; init; }
}