using System.Security.Claims;
using IdentityModel;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Overoom.IntegrationEvents.Users;
using PJMS.AuthService.Abstractions.Accessories;
using PJMS.AuthService.Abstractions.AppEmailService;
using PJMS.AuthService.Abstractions.AppEmailService.Structs;
using PJMS.AuthService.Abstractions.Commands.Create;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Extensions;

namespace PJMS.AuthService.Services.Commands.Create;

/// <summary>
/// Обработчик для выполнения команды создания пользователя.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
/// <param name="emailService">Сервис электронной почты для отправки уведомлений.</param>
public class CreateUserCommandHandler(UserManager<AppUser> userManager, IEmailService emailService, IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreateUserCommand, AppUser>
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
    public async Task<AppUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Создание нового объекта пользователя на основе данных из запроса.
        var user = new AppUser
        {
            // Задает имя пользователя, извлекаемое из электронной почты путем разделения строки по символу '@' и выбора первой части.
            UserName = request.Email.Split('@')[0].CutTo(40),

            // Задает электронную почту пользователя.
            Email = request.Email,

            // Задает время регистрации пользователя в формате UTC.
            TimeRegistration = DateTime.UtcNow,

            // Задает время последней аутентификации пользователя в формате UTC.
            TimeLastAuth = DateTime.UtcNow,

            // Задает локаль пользователя.
            Locale = request.Locale
        };


        // Попытка создания пользователя с использованием UserManager.
        var result = await userManager.CreateAsync(user, request.Password);

        // Проверка успешности создания пользователя
        if (!result.Succeeded)
        {
            // Если хоть одна ошибка DuplicateEmail, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "DuplicateEmail")) throw new EmailAlreadyTakenException();

            // Если хоть одна ошибка InvalidEmail, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "InvalidEmail")) throw new EmailFormatException();

            // Если хоть одна ошибка InvalidUserNameLength, то вызываем исключение 
            if (result.Errors.Any(error => error.Code == "InvalidUserNameLength")) throw new UserNameLengthException();

            // Создаем словарь для хранения ошибок
            var passwordValidationErrors = result.Errors.ToDictionary(e => e.Code, e => e.Description);

            // Вызываем исключение, содержащие в себе словарь ошибок валидации пароля
            throw new PasswordValidationException { ValidationErrors = passwordValidationErrors };
        }

        // Генерация кода подтверждения и формирование URL для подтверждения электронной почты.
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        // Так же добавляем локализацию в утверждения пользователя
        await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Locale, user.Locale.GetLocalizationString()));

        // Генерация URL подтверждения почты
        var url = GenerateMailUrl(request.ConfirmUrl, user.Id, code);

        // Отправка электронного письма с ссылкой для подтверждения регистрации.
        await emailService.SendAsync(new ConfirmRegistrationEmail { Recipient = request.Email, ConfirmLink = url });

        // Публикуем событие
        await publishEndpoint.Publish(new UserCreatedIntegrationEvent
        {
            Id = user.Id,
            PhotoUrl = user.Thumbnail,
            Name = user.UserName,
            Email = user.Email!,
            Locale = user.Locale.ToString()
        }, cancellationToken);
        
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
    private static string GenerateMailUrl(string url, Guid id, string code)
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