using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Commands.Password;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.Password;

namespace PJMS.AuthService.Tests.Commands.Password;

/// <summary>
/// Тестовый класс для ChangePasswordCommandHandler.
/// </summary>
public class ChangePasswordCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly ChangePasswordCommandHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChangePasswordCommandHandlerTest()
    {
        // Инициализация mock объекта UserManager.
        _userManagerMock = new Mock<UserManager<AppUser>>(
            new Mock<IUserStore<AppUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<AppUser>>().Object,
            Array.Empty<IUserValidator<AppUser>>(),
            Array.Empty<IPasswordValidator<AppUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<AppUser>>>().Object);

        // Инициализация обработчика.
        _handler = new ChangePasswordCommandHandler(_userManagerMock.Object);
    }

    /// <summary>
    /// Проверка валидной команды для изменения пароля у пользователя.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ChangePassword()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByIdAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
            {
                UserName = "test",
                Email = "test@example.com",
                TimeRegistration = DateTime.UtcNow,
                TimeLastAuth = DateTime.UtcNow,
                Locale = Localization.Az
            });

        // Настройка mock объекта UserManager для успешного добавления пароля при вызове AddPasswordAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.AddPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))

            // Возвращаем Success.
            .ReturnsAsync(IdentityResult.Success);

        // Настройка mock объекта UserManager для успешной смены пароля при вызове ChangePasswordAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ChangePasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))

            // Возвращаем Success.
            .ReturnsAsync(IdentityResult.Success);

        // Создаем команду для изменения пароля у пользователя.
        var command = new ChangePasswordCommand("old_password", "new_password")
        {
            UserId = Guid.NewGuid()
        };

        // Act
        // Вызов обработчика команды и ожидание возникновения исключения (если такое есть).
        var exception = await Record.ExceptionAsync(async () =>
        {
            await _handler.Handle(command, CancellationToken.None);
        });

        // Assert
        // Проверка на отсутствие исключения.
        Assert.Null(exception);
    }

    /// <summary>
    /// Проверка случая, когда пользователь не найден по id.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserNotFoundById_ThrowsUserNotFoundException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения null при вызове FindByLoginAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем null
            .ReturnsAsync(() => null);

        // Создаем команду для изменения пароля у пользователя.
        var command = new ChangePasswordCommand("old_password", "new_password")
        {
            UserId = Guid.NewGuid()
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка случая, когда пользователь не ввел старый пароль.
    /// </summary>
    [Fact]
    public async Task Handle_WhenOldPasswordIsNull_ThrowsOldPasswordNeededException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByIdAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем "тестового" пользователя.
            .ReturnsAsync(() => new AppUser
            {
                UserName = "test",
                Email = "test@example.com",
                TimeRegistration = DateTime.UtcNow,
                TimeLastAuth = DateTime.UtcNow,
                Locale = Localization.Az,

                // Также указываем хэш, чтобы дойти до условия проверки в обработчике.
                PasswordHash = "test_hash"
            });

        // Создаем команду для изменения пароля у пользователя.
        var command = new ChangePasswordCommand(null, "new_password")
        {
            UserId = Guid.NewGuid()
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<OldPasswordNeededException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка случая, когда пароль не прошел валидацию.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidPassword_ThrowsPasswordValidationException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByIdAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
            {
                UserName = "test",
                Email = "test@example.com",
                TimeRegistration = DateTime.UtcNow,
                TimeLastAuth = DateTime.UtcNow,
                Locale = Localization.Az
            });

        // Настройка mock объекта UserManager для возврата неудачного результата при вызове AddPasswordAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.AddPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))

            // Возвращаем результат выполнения метода -> пароль не прошел валидацию.
            .ReturnsAsync(IdentityResult.Failed());

        // Настройка mock объекта UserManager для неудачного результата при вызове ChangePasswordAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ChangePasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))

            // Возвращаем результат выполнения метода -> пароль не прошел валидацию.
            .ReturnsAsync(IdentityResult.Failed());

        // Создаем команду для изменения пароля у пользователя.
        var command = new ChangePasswordCommand("old_password", "new_password")
        {
            UserId = Guid.NewGuid()
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<PasswordValidationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}