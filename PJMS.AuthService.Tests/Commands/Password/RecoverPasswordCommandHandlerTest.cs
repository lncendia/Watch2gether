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
/// Тестовый класс для RecoverPasswordCommandHandler.
/// </summary>
public class RecoverPasswordCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly RecoverPasswordCommandHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RecoverPasswordCommandHandlerTest()
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
        _handler = new RecoverPasswordCommandHandler(_userManagerMock.Object);
    }
    
    /// <summary>
    /// Проверка валидной команды восстановления пароля у пользователя.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_RecoverPassword()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByEmailAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
        {
            UserName = "test",
            Email = "test@example.com",
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        });

        // Настройка mock объекта UserManager для возвращения успеха при вызове ResetPasswordAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ResetPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем Success.
            .ReturnsAsync(IdentityResult.Success);
        
        // Создаем команду для восстановления пароля у пользователя.
        var command = new RecoverPasswordCommand
        {
            Email = "test@example.com",
            Code = "test_code",
            NewPassword = "new_password"
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
    /// Проверка случая, когда пользователь не найден по почте.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserNotFoundByEmail_ThrowsUserNotFoundException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения null при вызове FindByLoginAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            
            // Возвращаем null
            .ReturnsAsync(() => null);
            
        // Создаем команду для восстановления пароля у пользователя.
        var command = new RecoverPasswordCommand
        {
            Email = "test@example.com",
            Code = "test_code",
            NewPassword = "new_password"
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда пользователь ввел неверный код подтверждения.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidCode_ThrowsInvalidCodeException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByIdAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
        {
            UserName = "test",
            Email = "test@example.com",
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        });
        
        // Настройка mock объекта UserManager для возврата ошибки с кодом DuplicateEmail при вызове ResetPasswordAsync.
        _userManagerMock
            
            // Выбираем метод, к которому делаем заглушку. 
            .Setup(m => m.ResetPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем ошибку с кодом InvalidCode.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "InvalidToken" }));
        
        // Создаем команду для восстановления пароля у пользователя.
        var command = new RecoverPasswordCommand
        {
            // Задаем "тестовую" почту.
            Email = "test@example.com",
            
            // Задаем "тестовый" код для восстановления пароля.
            Code = "test_code",
            
            // Задаем новый пароль пользователя.
            NewPassword = "new_password"
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения InvalidCodeException.
        await Assert.ThrowsAsync<InvalidCodeException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда пользователь не найден по почте.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidPassword_ThrowsPasswordValidationException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByIdAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
        {
            UserName = "test",
            Email = "test@example.com",
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        });
        
        // Настройка mock объекта UserManager для неудачного результата при вызове ResetPasswordAsync.
        _userManagerMock
            
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ResetPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем результат выполнения метода -> пароль не прошел валидацию.
            .ReturnsAsync(IdentityResult.Failed());
        
        // Создаем команду для восстановления пароля у пользователя.
        var command = new RecoverPasswordCommand
        {
            Email = "test@example.com",
            Code = "test_code",
            NewPassword = "new_password"
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения PasswordValidationException.
        await Assert.ThrowsAsync<PasswordValidationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}