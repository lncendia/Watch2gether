using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Commands.Authentication;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.Authentication;

namespace PJMS.AuthService.Tests.Commands.Authentication;

/// <summary>
/// Тестовый класс для AuthenticateUserByExternalProviderCommandHandler
/// </summary>
public class AuthenticateUserByExternalProviderCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly AuthenticateUserByExternalProviderCommandHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AuthenticateUserByExternalProviderCommandHandlerTest()
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
        _handler = new AuthenticateUserByExternalProviderCommandHandler(_userManagerMock.Object);
    }
        
    /// <summary>
    /// Проверка валидной команды на аутентификацию.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_Authenticate()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByLoginAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
        {
            UserName = "test",
            Email = "test@example.com",
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        });
            
        // Настройка mock объекта UserManager для возвращения false при вызове IsLockedOutAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.IsLockedOutAsync(It.IsAny<AppUser>()))
            
            // Возвращаем false.
            .ReturnsAsync(() => false);

        // Создаем команду для аутентификации пользователя через внешний провайдер.
        var command = new AuthenticateUserByExternalProviderCommand
        {
            // Задаем "тестовый" провайдер.
            LoginProvider = "TestProvider",
            
            // Задаем "тестовый" ключ провайдера.
            ProviderKey = "TestKey"
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
    /// Проверка случая, когда пользователь не найден по провайдеру.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserNotFoundFromProvider_ThrowsUserNotFoundException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения null при вызове FindByLoginAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем null
            .ReturnsAsync(() => null);
            
        // Создаем команду для аутентификации пользователя через внешний провайдер.
        var command = new AuthenticateUserByExternalProviderCommand
        {
            // Задаем "тестовый" провайдер.
            LoginProvider = "TestProvider",
            
            // Задаем "тестовый" ключ провайдера.
            ProviderKey = "TestKey"
        };
            
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка случая, когда пользователь заблокирован.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserIsLockout_ThrowsUserLockoutException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByLoginAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
        {
            UserName = "test",
            Email = "test@example.com",
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        });

        // Настройка mock объекта UserManager для возвращения true при вызове IsLockedOutAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.IsLockedOutAsync(It.IsAny<AppUser>()))
            
            // Возвращаем true, так как пользователь должен быть временно заблокирован. 
            .ReturnsAsync(() => true);
            
        // Создаем команду для аутентификации пользователя через внешний провайдер.
        var command = new AuthenticateUserByExternalProviderCommand
        {
            // Задаем "тестовый" провайдер.
            LoginProvider = "TestProvider",
            
            // Задаем "тестовый" ключ провайдера.
            ProviderKey = "TestKey"
        };
            
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserLockoutException.
        await Assert.ThrowsAsync<UserLockoutException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
        
}