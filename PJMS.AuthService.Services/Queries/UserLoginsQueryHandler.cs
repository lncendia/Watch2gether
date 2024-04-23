using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Abstractions.Queries;

namespace PJMS.AuthService.Services.Queries;

/// <summary>
/// Обработчик запроса на получение списка внешних учетных записей пользователя.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class UserLoginsQueryHandler(UserManager<AppUser> userManager)
    : IRequestHandler<UserLoginsQuery, IReadOnlyCollection<string>>
{
    /// <summary>
    /// Метод обработки запроса на получение списка внешних учетных записей пользователя.
    /// </summary>
    /// <param name="request">Запрос на получение списка внешних учетных записей пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает список внешних учетных записей пользователя.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    public async Task<IReadOnlyCollection<string>> Handle(UserLoginsQuery request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору; 
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        
        // Если не найден, вызываем исключение UserNotFoundException.
        if(user == null) throw new UserNotFoundException();
        
        // Получение списка внешних учетных записей пользователя.
        var logins = await userManager.GetLoginsAsync(user);
        
        // Возвращение списка внешних учетных записей в виде массива строк.
        return logins.Select(info => info.LoginProvider).ToArray();
    }
}