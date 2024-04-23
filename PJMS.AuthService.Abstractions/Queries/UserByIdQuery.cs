using MediatR;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Queries;

/// <summary>
/// Запрос для получения пользователя по идентификатору.
/// </summary>
public class UserByIdQuery : IRequest<AppUser>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required Guid Id { get; init; }
}