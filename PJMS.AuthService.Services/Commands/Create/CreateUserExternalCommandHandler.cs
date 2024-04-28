using System.Security.Claims;
using IdentityModel;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Overoom.IntegrationEvents.Users;
using PJMS.AuthService.Abstractions.Accessories;
using PJMS.AuthService.Abstractions.AppThumbnailStore;
using PJMS.AuthService.Abstractions.Commands.Create;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Extensions;

namespace PJMS.AuthService.Services.Commands.Create;

/// <summary>
/// Обработчик команды создания пользователя с внешней учетной записью.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class CreateUserExternalCommandHandler(
    UserManager<AppUser> userManager,
    IThumbnailStore thumbnailStore,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreateUserExternalCommand, AppUser>
{
    private const string ThumbnailClaimType = "photo:link";

    /// <summary>
    /// Метод обработки команды создания пользователя с внешней учетной записью.
    /// </summary>
    /// <param name="request">Запрос создания пользователя с внешней учетной записью.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает созданного пользователя в случае успеха.</returns>
    /// <exception cref="EmailFormatException">Вызывается, если почта имеет некорректный формат.</exception>
    /// <exception cref="EmailAlreadyTakenException">Вызывается, если почта уже используется другим пользователем.</exception>
    /// <exception cref="LoginAlreadyAssociatedException">Вызывается, если логин связан с другим пользователем.</exception>
    public async Task<AppUser> Handle(CreateUserExternalCommand request, CancellationToken cancellationToken)
    {
        // Пытаемся получить пользователя по логину
        var loginUser =
            await userManager.FindByLoginAsync(request.LoginInfo.LoginProvider, request.LoginInfo.ProviderKey);

        // Если пользователь существует вызываем исключение
        if (loginUser != null) throw new LoginAlreadyAssociatedException();

        // Пытаемся получить почту из утверждений, если почты нет - вызываем исключение
        var email = request.LoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ?? throw new EmailFormatException();

        // Пытаемся получить имя пользователя из утверждений, если нет - сплитим почту
        var username = request.LoginInfo.Principal.FindFirstValue(ClaimTypes.Name) ?? email.Split('@')[0];

        // Создаем пользователя
        var user = new AppUser
        {
            // Задаем электронную почту пользователя
            Email = email,

            // Задаем имя пользователя
            UserName = username.CutTo(40),

            // Задаем время регистрации пользователя в формате UTC
            TimeRegistration = DateTime.UtcNow,

            // Задаем время последней аутентификации пользователя в формате UTC
            TimeLastAuth = DateTime.UtcNow,

            // Задаем локаль пользователя
            Locale = request.Locale,

            // Устанавливаем подтверждение электронной почты пользователя в значение true
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

            // Если хоть одна ошибка InvalidUserNameLength, то вызываем исключение 
            if (result.Errors.Any(error => error.Code == "InvalidUserNameLength")) throw new UserNameLengthException();
        }

        // Так же добавляем локализацию в утверждения пользователя
        await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Locale, user.Locale.GetLocalizationString()));

        // Пытаемся получить ссылку на аватар из утверждений
        var thumbnailClaim = request.LoginInfo.Principal.FindFirstValue(ThumbnailClaimType);

        // Если аватар есть в утверждениях, то сохраняем его локально
        if (thumbnailClaim != null)
        {
            try
            {
                // Переменная thumbnail будет содержать результат сохранения миниатюры 
                user.Thumbnail = await thumbnailStore.SaveAsync(new Uri(thumbnailClaim), user.Id);
                
                // Так же добавляем фото профиля в утверждения пользователя
                await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Picture, user.Thumbnail.ToString()));
            }
            catch (ThumbnailSaveException)
            {
                // Если возникла ошибка ThumbnailSaveException, игнорируем ее и продолжаем выполнение
            }
        }

        // Связываем пользователя с внешним провайдером
        await userManager.AddLoginAsync(user, request.LoginInfo);

        // Публикуем событие
        await publishEndpoint.Publish(new UserCreatedIntegrationEvent
        {
            Id = user.Id,
            PhotoUrl = user.Thumbnail,
            Name = user.UserName,
            Email = user.Email!,
            Locale = user.Locale.ToString()
        }, cancellationToken);

        // Возвращаем пользователя
        return user;
    }
}