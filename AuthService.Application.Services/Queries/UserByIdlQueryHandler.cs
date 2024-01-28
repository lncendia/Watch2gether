using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using AuthService.Application.Abstractions.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Queries;

/// <summary>
/// Обработчик запроса на получение пользователя по идентификатору.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class UserByIdQueryHandler(UserManager<UserData> userManager) : IRequestHandler<UserByIdQuery, UserData>
{
    /// <summary>
    /// Метод обработки запроса на получение пользователя по идентификатору.
    /// </summary>
    /// <param name="request">Запрос на получение пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает пользователя по указанному идентификатору.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    public async Task<UserData> Handle(UserByIdQuery request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору; если не найден, вызываем исключение UserNotFoundException.
        return await userManager.FindByIdAsync(request.Id.ToString()) ?? throw new UserNotFoundException();
    }
}