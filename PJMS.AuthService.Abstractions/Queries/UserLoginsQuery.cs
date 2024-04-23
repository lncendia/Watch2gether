using MediatR;

namespace PJMS.AuthService.Abstractions.Queries;

/// <summary>
/// Запрос для получения списка внешних аутентификаций пользователя.
/// </summary>
public class UserLoginsQuery : IRequest<IReadOnlyCollection<string>>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required Guid Id { get; init; }
}