using System.Security.Claims;
using AuthService.Application.Abstractions;
using AuthService.Application.Abstractions.Abstractions.AppThumbnailStore;
using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Commands;

/// <summary>
/// Обработчик команды создания пользователя с внешней учетной записью.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
/// <param name="thumbnailStore">Хранилище фотографий.</param>
public class CreateUserExternalCommandHandler(UserManager<UserData> userManager, IThumbnailStore thumbnailStore)
    : IRequestHandler<CreateUserExternalCommand, UserData>
{
    private const string ThumbnailClaimType = "photo_thumb:link";

    /// <summary>
    /// Метод обработки команды создания пользователя с внешней учетной записью.
    /// </summary>
    /// <param name="request">Запрос создания пользователя с внешней учетной записью.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает созданного пользователя в случае успеха.</returns>
    /// <exception cref="EmailFormatException">Вызывается, если почта имеет некорректный формат.</exception>
    /// <exception cref="EmailAlreadyTakenException">Вызывается, если почта уже используется другим пользователем.</exception>
    /// <exception cref="LoginAlreadyAssociatedException">Вызывается, если логин связан с другим пользователем.</exception>
    /// <exception cref="UserNameFormatException">Вызывается, если имя пользователя имеет некорректный формат.</exception>
    /// <exception cref="UserNameLengthException">Вызывается, если имя пользователя имеет некорректную длину.</exception>
    public async Task<UserData> Handle(CreateUserExternalCommand request, CancellationToken cancellationToken)
    {
        // Пытаемся получить пользователя по логину
        var loginUser =
            await userManager.FindByLoginAsync(request.LoginInfo.LoginProvider, request.LoginInfo.ProviderKey);

        // Если пользователь существует вызываем исключение
        if (loginUser != null) throw new LoginAlreadyAssociatedException();

        // Пытаемся получить почту из утверждений, если почты нет - вызываем исключение
        var email = request.LoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ?? throw new EmailFormatException();

        // Пытаемся получить почту из утверждений, если почты нет - вызываем исключение
        var username = request.LoginInfo.Principal.FindFirstValue(ClaimTypes.Name) ??
                       throw new UserNameFormatException();

        // Пытаемся получить ссылку на аватар из утверждений
        var thumbnailClaim = request.LoginInfo.Principal.FindFirstValue(ThumbnailClaimType);

        // Переменная ссылки на аватар
        Uri thumbnail;
        
        // Если аватар есть в утверждениях, то сохраняем его локально
        if (thumbnailClaim != null) thumbnail = await thumbnailStore.SaveAsync(new Uri(thumbnailClaim));
        
        // Иначе устанавливаем аватар по умолчанию
        else thumbnail = ApplicationConstants.DefaultAvatar;

        // Создаем пользователя
        var user = new UserData(username, email, thumbnail, DateTime.UtcNow)
        {
            // Указываем, что почта подтверждена
            EmailConfirmed = true
        };

        // Сохраняем пользователя
        var result = await userManager.CreateAsync(user);

        // Если результат неудачный
        if (!result.Succeeded)
        {
            // Если хоть одна ошибка DuplicateEmail, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "DuplicateEmail")) throw new EmailAlreadyTakenException();

            // Если хоть одна ошибка InvalidEmail, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "InvalidEmail")) throw new EmailFormatException();

            // Если хоть одна ошибка InvalidUserName, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "InvalidUserName")) throw new EmailFormatException();

            // Если хоть одна ошибка InvalidUserNameLength, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "InvalidUserNameLength")) throw new UserNameLengthException();
        }

        // Добавляем аватар в утверждения
        await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Picture, user.AvatarUrl.ToString()));

        // Связываем пользователя с внешним провайдером
        await userManager.AddLoginAsync(user, request.LoginInfo);

        // Возвращаем пользователя
        return user;
    }
}