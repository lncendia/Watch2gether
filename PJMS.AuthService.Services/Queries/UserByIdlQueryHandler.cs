using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Abstractions.Queries;

namespace PJMS.AuthService.Services.Queries;

/// <summary>
/// Обработчик запроса на получение пользователя по идентификатору.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class UserByIdQueryHandler(UserManager<AppUser> userManager) : IRequestHandler<UserByIdQuery, AppUser>
{
    /// <summary>
    /// Метод обработки запроса на получение пользователя по идентификатору.
    /// </summary>
    /// <param name="request">Запрос на получение пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает пользователя по указанному идентификатору.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    public async Task<AppUser> Handle(UserByIdQuery request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору; если не найден, вызываем исключение UserNotFoundException.
        return await userManager.FindByIdAsync(request.Id.ToString()) ?? throw new UserNotFoundException();
    }
}