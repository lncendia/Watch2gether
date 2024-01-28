using AuthService.Application.Abstractions.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда для добавления внешней аутентификации пользователю.
/// </summary>
public class AddUserExternalLoginCommand : IRequest<UserData>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required long Id { get; init; }

    /// <summary>
    /// Получает или задает информацию о внешней аутентификации.
    /// </summary>
    public required ExternalLoginInfo LoginInfo { get; init; }
}