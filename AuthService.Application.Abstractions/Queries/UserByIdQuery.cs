using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Queries;

/// <summary>
/// Запрос для получения пользователя по идентификатору.
/// </summary>
public class UserByIdQuery : IRequest<UserData>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required long Id { get; init; }
}