using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Commands.TwoFactor;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.TwoFactor;

namespace PJMS.AuthService.Tests.Commands.TwoFactor;

/// <summary>
/// Тестовый класс для ResetTwoFactorCommandHandler
/// </summary>
public class ResetTwoFactorCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly ResetTwoFactorCommandHandler _handler;
    
    /// <summary>
    /// Конструктор.
    /// </summary>
    public ResetTwoFactorCommandHandlerTest()
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
        _handler = new ResetTwoFactorCommandHandler(_userManagerMock.Object);
    }

    /// <summary>
    /// Проверка валидной команды для сброса 2FA.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_Reset()
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

        // Настройка mock объекта UserManager для возвращения true при вызове GetTwoFactorEnabledAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetTwoFactorEnabledAsync(It.IsAny<AppUser>()))
            
            // Возвращаем true -> 2FA включена у пользователя.
            .ReturnsAsync(() => true);
        
        // Настройка mock объекта UserManager для возвращения true при вызове VerifyTwoFactorTokenAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.   
            .Setup(m => m.VerifyTwoFactorTokenAsync
                (It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))

            // Возвращаем тестового true => токен верифицирован. 
            .ReturnsAsync(() => true);
        
        // Создаем команду для сброса 2FA.
        var command = new ResetTwoFactorCommand
        {
            // Задаем id пользователя.
            UserId = Guid.NewGuid(),
            
            // Задаем код для прохождения 2FA.
            Code = "test_code",
            
            // Задаем тип кода.
            Type = CodeType.Authenticator
        };
        
        //Act
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
    /// Проверка валидной команды для сброса 2FA с помощью кода восстановления.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommandWithRecoveryCode_Reset()
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
        
        // Настройка mock объекта UserManager для возвращения true при вызове GetTwoFactorEnabledAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetTwoFactorEnabledAsync(It.IsAny<AppUser>()))
            
            // Возвращаем true -> 2FA включена у пользователя.
            .ReturnsAsync(() => true);
        
        // Настройка mock объекта UserManager для успешного результата выполнения RedeemTwoFactorRecoveryCodeAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.RedeemTwoFactorRecoveryCodeAsync
                (It.IsAny<AppUser>(), It.IsAny<string>()))

            // Выбираем Success -> код восстановления прошел проверку.  
            .ReturnsAsync(() => IdentityResult.Success);

        // Создаем команду для сброса 2FA.
        var command = new ResetTwoFactorCommand
        {
            // Задаем id пользователя.
            UserId = Guid.NewGuid(),
            
            // Задаем код для прохождения 2FA.
            Code = "test_code",
            
            // Задаем тип кода.
            Type = CodeType.RecoveryCode
        };
        
        //Act
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
            
        // Создаем команду для сброса 2FA.
        var command = new ResetTwoFactorCommand
        {
            // Задаем id пользователя.
            UserId = Guid.NewGuid(),
            
            // Задаем код для прохождения 2FA.
            Code = "test_code",
            
            // Задаем тип кода.
            Type = CodeType.Authenticator
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда тип кода для прохождения 2FA является невалидным.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidTypeCodeFor2FA_ThrowsArgumentOutOfRangeException()
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
        
        // Настройка mock объекта UserManager для возвращения true при вызове GetTwoFactorEnabledAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetTwoFactorEnabledAsync(It.IsAny<AppUser>()))
            
            // Возвращаем true -> 2FA включена у пользователя.
            .ReturnsAsync(() => true);
        
        // Создаем команду для сброса 2FA.
        var command = new ResetTwoFactorCommand
        {
            // Задаем id пользователя.
            UserId = Guid.NewGuid(),
            
            // Задаем код для прохождения 2FA.
            Code = "test_code",
            
            // Задаем тип кода.
            Type = (CodeType)100
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда код для прохождения 2FA является невалидным.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidCode_ThrowsInvalidCodeException()
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
        
        // Настройка mock объекта UserManager для возвращения true при вызове GetTwoFactorEnabledAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetTwoFactorEnabledAsync(It.IsAny<AppUser>()))
            
            // Возвращаем true -> 2FA включена у пользователя.
            .ReturnsAsync(() => true);
        
        // Настройка mock объекта UserManager для возвращения false пользователя при вызове VerifyTwoFactorTokenAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.VerifyTwoFactorTokenAsync
                (It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))

            // Возвращаем false -> токен не верифицирован.  
            .ReturnsAsync(() => false);
        
        // Создаем команду для сброса 2FA.
        var command = new ResetTwoFactorCommand
        {
            // Задаем id пользователя.
            UserId = Guid.NewGuid(),
            
            // Задаем код для прохождения 2FA.
            Code = "test_code",
            
            // Задаем тип кода.
            Type = CodeType.Authenticator
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения InvalidCodeException.
        await Assert.ThrowsAsync<InvalidCodeException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда код восстановления для прохождения 2FA является невалидным.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidCodeWithByRecoveryCode_ThrowsInvalidCodeException()
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
        
        // Настройка mock объекта UserManager для возвращения true при вызове GetTwoFactorEnabledAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetTwoFactorEnabledAsync(It.IsAny<AppUser>()))
            
            // Возвращаем true -> 2FA включена у пользователя.
            .ReturnsAsync(() => true);
        
        // Настройка mock объекта UserManager для возвращения неудачного результата выполнения RedeemTwoFactorRecoveryCodeAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.RedeemTwoFactorRecoveryCodeAsync
                (It.IsAny<AppUser>(), It.IsAny<string>()))

            // Возвращаем неудачный результат выполнения -> код не верифицирован.  
            .ReturnsAsync(() => IdentityResult.Failed());
        
        // Создаем команду для сброса 2FA.
        var command = new ResetTwoFactorCommand
        {
            // Задаем id пользователя.
            UserId = Guid.NewGuid(),
            
            // Задаем код для прохождения 2FA.
            Code = "test_code",
            
            // Задаем тип кода.
            Type = CodeType.Authenticator
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения InvalidCodeException.
        await Assert.ThrowsAsync<InvalidCodeException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}