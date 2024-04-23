using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Commands.External;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.External;

namespace PJMS.AuthService.Tests.Commands.External;

/// <summary>
/// Тестовый класс для RemoveUserExternalLoginCommandHandler.
/// </summary>
public class RemoveUserExternalLoginCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly RemoveUserExternalLoginCommandHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveUserExternalLoginCommandHandlerTest()
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
        _handler = new RemoveUserExternalLoginCommandHandler(_userManagerMock.Object);
    }

    /// <summary>
    /// Проверка валидной команды удаления внешней аутентификации пользователя.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_RemoveExternalLogin()
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

        // Настройка mock объекта UserManager для возврата списка уже существующих провайдеров.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetLoginsAsync(It.IsAny<AppUser>()))

            // Возвращаем тестовый список провайдеров.
            .ReturnsAsync(() => new List<UserLoginInfo>(new[]
            {
                // Создаем тестового провайдера.
                new UserLoginInfo("TestProvider", "TestKey", "TestDisplayName")
            }));

        // Создаем команду для удаления внешней аутентификации пользователя.
        var command = new RemoveUserExternalLoginCommand
        {
            // Задаем Id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем провайдер внешней аутентификации.
            Provider = "TestProvider"
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
    /// Проверка случая, когда пользователь не найден по Id.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserNotFoundById_ThrowsUserNotFoundException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByIdAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => null);

        // Создаем команду для удаления внешней аутентификации пользователя.
        var command = new RemoveUserExternalLoginCommand
        {
            // Задаем Id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем провайдер внешней аутентификации.
            Provider = "TestProvider"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка случая, когда внешний логин не найден по провайдеру.
    /// </summary>
    [Fact]
    public async Task Handle_WhenLoginFromProviderNotFound_ThrowsLoginNotFoundException()
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

        // Настройка mock объекта UserManager для возврата списка уже существующих провайдеров.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetLoginsAsync(It.IsAny<AppUser>()))

            // Возвращаем тестовый список провайдеров.
            .ReturnsAsync(() => new List<UserLoginInfo>());

        // Создаем команду для удаления внешней аутентификации пользователя.
        var command = new RemoveUserExternalLoginCommand
        {
            // Задаем Id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем провайдер внешней аутентификации.
            Provider = "TestProvider"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<LoginNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}