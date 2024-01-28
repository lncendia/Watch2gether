using AuthService.Application.Abstractions.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда для создания пользователя через внешний провайдер.
/// </summary>
public class CreateUserExternalCommand : IRequest<UserData>
{
    /// <summary>
    /// Получает или задает информацию о внешней аутентификации.
    /// </summary>
    public required ExternalLoginInfo LoginInfo { get; init; }
}