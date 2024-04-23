using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Services.Validators;

namespace PJMS.AuthService.Tests.Validators;

/// <summary>
/// Тестовый класс для CustomPasswordValidator.
/// </summary>
public class CustomPasswordValidatorTest
{
    /// <summary>
    /// Поле валидатора.
    /// </summary>
    private readonly CustomPasswordValidator _customPasswordValidator = new();

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
    /// Поле пользователя.
    /// </summary>
    private readonly AppUser _appUser = new()
    {
        UserName = "test",
        Email = "test@example.com",
        TimeRegistration = DateTime.UtcNow,
        TimeLastAuth = DateTime.UtcNow,
        Locale = Localization.Az
    };

    /// <summary>
    /// Проверка валидного пароля.
    /// </summary>
    /// <param name="password">Пароль.</param>
    [Theory]
    [InlineData("12Pa$$Wor0923")]
    [InlineData("12Pa$$Wo")]
    [InlineData("7OVipW1#w")]
    [InlineData("d!rT#Yk5")]
    public async Task ValidateAsync_ValidPassword(string password)
    {
        // Act
        // Валидация пароля
        var result = await _customPasswordValidator.ValidateAsync(_userManagerMock.Object, _appUser, password);

        // Assert
        // Проверяем является ли тип результата нашим ожиданием.
        Assert.Equal(IdentityResult.Success, result);
    }

    /// <summary>
    /// Проверка пароля не подходящего по длине.
    /// </summary>
    /// <param name="password">Пароль.</param>
    [Theory]
    [InlineData("12345")]
    [InlineData("123456")]
    [InlineData("1234567")]
    [InlineData(
        "gIxsg69xJ%uO8V-AlsiOOwHKjUJHu2F49bqO5xtm3r8Gad#Iyu2Ffu8-h5VrgIxsg69xJ%uO8V-AlsiOOwHKjUJHu2F49bqO5xtm3r8Gad#Iyu2Ffu8-h5VrgIxsg69xJ%uO8V-AlsiOOwHKjUJHu2F49bqO5xtm3r8Gad#Iyu2Ffu8-h5Vr")]
    public async Task ValidateAsync_InvalidLengthPassword(string password)
    {
        // Act
        // Валидация пароля
        var result = await _customPasswordValidator.ValidateAsync(_userManagerMock.Object, _appUser, password);

        // Assert
        // Проверяем является ли код ошибки тем, который мы ожидали
        Assert.Equal("PasswordLengthInvalid", result.Errors.FirstOrDefault()?.Code);
    }

    /// <summary>
    /// Проверка пароля без букв в верхнем регистре.
    /// </summary>
    /// <param name="password">Пароль.</param>
    [Theory]
    [InlineData("12pa$$wor0923")]
    [InlineData("12pa$$wo")]
    [InlineData("7avi1#wop")]
    [InlineData("d!rt#yk5")]
    public async Task ValidateAsync_WithoutUppersPassword(string password)
    {
        // Act
        // Валидация пароля
        var result = await _customPasswordValidator.ValidateAsync(_userManagerMock.Object, _appUser, password);

        // Assert
        // Проверяем является ли код ошибки тем, который мы ожидали
        Assert.Equal("PasswordRequiresUpper", result.Errors.FirstOrDefault()?.Code);
    }

    /// <summary>
    /// Проверка пароля без букв в нижнем регистре.
    /// </summary>
    /// <param name="password">Пароль.</param>
    [Theory]
    [InlineData("12PA$$WOR0923")]
    [InlineData("12PA$$WO")]
    [InlineData("7AVI1#WOP")]
    [InlineData("D!RT#YK5")]
    public async Task ValidateAsync_WithoutLowersPassword(string password)
    {
        // Act
        // Валидация пароля
        var result = await _customPasswordValidator.ValidateAsync(_userManagerMock.Object, _appUser, password);

        // Assert
        // Проверяем является ли код ошибки тем, который мы ожидали
        Assert.Equal("PasswordRequiresLower", result.Errors.FirstOrDefault()?.Code);
    }

    /// <summary>
    /// Проверка пароля без цифр.
    /// </summary>
    /// <param name="password">Пароль.</param>
    [Theory]
    [InlineData("zaPA$$WORokl")]
    [InlineData("zaPA$$WO")]
    [InlineData("aAVIl#WOP")]
    [InlineData("D!RT#YKl")]
    public async Task ValidateAsync_WithoutDigitsPassword(string password)
    {
        // Act
        // Валидация пароля
        var result = await _customPasswordValidator.ValidateAsync(_userManagerMock.Object, _appUser, password);

        // Assert
        // Проверяем является ли код ошибки тем, который мы ожидали
        Assert.Equal("PasswordRequiresDigit", result.Errors.FirstOrDefault()?.Code);
    }

    /// <summary>
    /// Проверка пароля без специальных символов.
    /// </summary>
    /// <param name="password">Пароль.</param>
    [Theory]
    [InlineData("zaPASSWORD34l")]
    [InlineData("zaPASSWORD34")]
    [InlineData("aAVIlWOPI123")]
    [InlineData("RUDYKl09")]
    public async Task ValidateAsync_WithoutSpecialCharsPassword(string password)
    {
        // Act
        // Валидация пароля
        var result = await _customPasswordValidator.ValidateAsync(_userManagerMock.Object, _appUser, password);

        // Assert
        // Проверяем является ли код ошибки тем, который мы ожидали
        Assert.Equal("PasswordRequiresNonAlphanumeric", result.Errors.FirstOrDefault()?.Code);
    }
}