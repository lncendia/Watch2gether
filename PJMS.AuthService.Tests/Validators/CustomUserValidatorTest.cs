using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Services.Validators;

namespace PJMS.AuthService.Tests.Validators;

/// <summary>
/// Тестовый класс для CustomUserValidator.
/// </summary>
public class CustomUserValidatorTest
{
    /// <summary>
    /// Поле валидатора.
    /// </summary>
    private readonly CustomUserValidator _customUserValidator = new();

    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock = new(
        new Mock<IUserStore<AppUser>>().Object,
        new Mock<IOptions<IdentityOptions>>().Object,
        new Mock<IPasswordHasher<AppUser>>().Object,
        Array.Empty<IUserValidator<AppUser>>(),
        Array.Empty<IPasswordValidator<AppUser>>(),
        new Mock<ILookupNormalizer>().Object,
        new Mock<IdentityErrorDescriber>().Object,
        new Mock<IServiceProvider>().Object,
        new Mock<ILogger<UserManager<AppUser>>>().Object);

    /// <summary>
    /// Проверка валидного почты.
    /// </summary>
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("test@yandex.net")]
    [InlineData("test@gmail.com")]
    [InlineData("test@mail.ru")]
    public async Task ValidateAsync_ValidEmail(string email)
    {
        // Arrange
        // Создаем экземпляр пользователя.
        var user = new AppUser
        {
            UserName = email.Split('@')[0],
            Email = email,
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        };

        // Настройка mock объекта UserManager для возвращения null при вызове FindByEmailAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.
            .Setup(m => m.FindByEmailAsync(email))

            // Возвращаем null. 
            .ReturnsAsync(() => null);

        // Act
        // Валидация почты
        var result = await _customUserValidator.ValidateAsync(_userManagerMock.Object, user);

        // Assert
        // Проверяем является ли тип результата нашим ожиданием.
        Assert.Equal(IdentityResult.Success, result);
    }
    
    /// <summary>
    /// Проверка невалидной почты.
    /// </summary>
    [Theory]
    [InlineData("")]
    public async Task ValidateAsync_InvalidEmailWithNullOrWhiteSpace(string email)
    {
        // Arrange
        // Создаем экземпляр пользователя.
        var user = new AppUser
        {
            UserName = email.Split('@')[0],
            Email = email,
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        };

        // Act
        // Валидация пароля
        var result = await _customUserValidator.ValidateAsync(_userManagerMock.Object, user);
        
        // Assert
        // Проверяем является ли код ошибки тем, который мы ожидали
        Assert.Equal("InvalidUserNameLength", result.Errors.FirstOrDefault()?.Code);
    }
    
    /// <summary>
    /// Проверка невалидной почты.
    /// </summary>
    [Theory]
    [InlineData("test")]
    [InlineData("test@")]
    [InlineData("@")]
    [InlineData("@mail.ru")]
    public async Task ValidateAsync_InvalidFormatEmail(string email)
    {
        // Arrange
        // Создаем экземпляр пользователя.
        var user = new AppUser
        {
            UserName = "test_user",
            Email = email,
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        };
        
        // Act
        // Валидация пароля
        var result = await _customUserValidator.ValidateAsync(_userManagerMock.Object, user);
        
        // Assert
        // Проверяем является ли код ошибки тем, который мы ожидали
        Assert.Equal("InvalidEmail", result.Errors.FirstOrDefault()?.Code);
    }
    
    /// <summary>
    /// Проверка валидного почты.
    /// </summary>
    [Theory]
    [InlineData("test@example.com")]
    public async Task ValidateAsync_WhenEmailAlreadyTaken(string email)
    {
        // Arrange
        // Создаем экземпляр пользователя.
        var user = new AppUser
        {
            UserName = email.Split('@')[0],
            Email = email,
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az,
            Id = Guid.NewGuid()
        };
        
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByEmailAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByEmailAsync(email))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
            {
                UserName = email.Split('@')[0],
                Email = email,
                TimeRegistration = DateTime.UtcNow,
                TimeLastAuth = DateTime.UtcNow,
                Locale = Localization.Az,
                Id = Guid.NewGuid()
            });

        // Act
        // Валидация пароля
        var result = await _customUserValidator.ValidateAsync(_userManagerMock.Object, user);
        
        // Assert
        // Проверяем является ли код ошибки тем, который мы ожидали
        Assert.Equal("DuplicateEmail", result.Errors.FirstOrDefault()?.Code);
    }
    
    /// <summary>
    /// Проверка невалидного имени пользователя.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("dsajdashjbcxzbnczxhgdhadsadassgdashdgashdasbcxgzvcgxzgvadgstdasdash@example.com")]
    public async Task ValidateAsync_WhenUsernameLengthIsInvalid(string username)
    {
        // Arrange
        // Создаем экземпляр пользователя.
        var user = new AppUser
        {
            UserName = username,
            Email = username + "@test.ru",
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az,
            Id = Guid.NewGuid()
        };
        
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByEmailAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByEmailAsync(username))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
            {
                UserName = username,
                Email = username + "@test.ru",
                TimeRegistration = DateTime.UtcNow,
                TimeLastAuth = DateTime.UtcNow,
                Locale = Localization.Az,
                Id = Guid.NewGuid()
            });

        // Act
        // Валидация пароля
        var result = await _customUserValidator.ValidateAsync(_userManagerMock.Object, user);
        
        // Assert
        // Проверяем является ли код ошибки тем, который мы ожидали
        Assert.Equal("InvalidUserNameLength", result.Errors.FirstOrDefault()?.Code);
    }
}