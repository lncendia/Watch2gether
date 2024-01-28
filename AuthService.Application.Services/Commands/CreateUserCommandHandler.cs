using System.Security.Claims;
using AuthService.Application.Abstractions;
using AuthService.Application.Abstractions.Abstractions.AppEmailService;
using AuthService.Application.Abstractions.Abstractions.AppEmailService.Structs;
using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Commands;

/// <summary>
/// Обработчик для выполнения команды создания пользователя.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
/// <param name="emailService">Сервис электронной почты для отправки уведомлений.</param>
public class CreateUserCommandHandler(UserManager<UserData> userManager, IEmailService emailService)
    : IRequestHandler<CreateUserCommand, UserData>
{
    /// <summary>
    /// Метод обработки команды создания пользователя.
    /// </summary>
    /// <param name="request">Запрос на создание пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает созданного пользователя в случае успеха.</returns>
    /// <exception cref="EmailAlreadyTakenException">Вызывается, если пользователь уже существует.</exception>
    /// <exception cref="EmailFormatException">Вызывается, если почта имеет неверный формат.</exception>
    /// <exception cref="PasswordValidationException">Вызывается, если валидация пароля не прошла.</exception>
    /// <exception cref="UserNameFormatException">Вызывается, если имя пользователя имеет некорректный формат.</exception>
    /// <exception cref="UserNameLengthException">Вызывается, если имя пользователя имеет некорректную длину.</exception>
    public async Task<UserData> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Создание нового объекта пользователя на основе данных из запроса.
        var user = new UserData(request.Username, request.Email, ApplicationConstants.DefaultAvatar, DateTime.UtcNow);

        // Попытка создания пользователя с использованием UserManager.
        var result = await userManager.CreateAsync(user, request.Password);

        // Проверка успешности создания пользователя
        if (!result.Succeeded)
        {
            // Если хоть одна ошибка DuplicateUserName, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "DuplicateEmail")) throw new EmailAlreadyTakenException();

            // Если хоть одна ошибка InvalidEmail, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "InvalidEmail")) throw new EmailFormatException();

            // Если хоть одна ошибка InvalidUserName, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "InvalidUserName")) throw new UserNameFormatException();

            // Если хоть одна ошибка InvalidUserNameLength, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "InvalidUserNameLength")) throw new UserNameLengthException();

            // Создаем словарь для хранения ошибок
            var passwordValidationErrors = result.Errors.ToDictionary(e => e.Code, e => e.Description);

            // Вызываем исключение, содержащие в себе словарь ошибок валидации пароля
            throw new PasswordValidationException { ValidationErrors = passwordValidationErrors };
        }

        // Добавляем аватар в утверждения
        await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Picture, user.AvatarUrl.ToString()));

        // Генерация кода подтверждения и формирование URL для подтверждения электронной почты.
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        // Формируем url подтверждения почты
        var url = GenerateMailUrl(request.ConfirmUrl, user.Id, code);

        // Отправка электронного письма с ссылкой для подтверждения регистрации.
        await emailService.SendAsync(new ConfirmRegistrationEmail { Recipient = request.Email, ConfirmLink = url });

        // Возвращение созданного пользователя.
        return user;
    }

    /// <summary>
    /// Генерирует URL для подтверждения регистрации по электронной почте.
    /// </summary>
    /// <param name="url">Базовый URL.</param>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="code">Код подтверждения.</param>
    /// <returns>Сгенерированный URL.</returns>
    private static string GenerateMailUrl(string url, long id, string code)
    {
        // Создаем объект UriBuilder с базовым URL
        var uriBuilder = new UriBuilder(url);

        // Получаем коллекцию параметров запроса
        var queryParameters = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

        // Добавляем параметр "id" со значением
        queryParameters["userId"] = id.ToString();

        // Добавляем параметр "code" со значением
        queryParameters["code"] = code;

        // Устанавливаем обновленную строку запроса
        uriBuilder.Query = queryParameters.ToString();

        // Получаем обновленный URL
        return uriBuilder.ToString();
    }
}