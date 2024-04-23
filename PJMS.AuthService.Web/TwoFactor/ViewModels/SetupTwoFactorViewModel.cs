using System.Text;
using System.Web;
using PJMS.AuthService.Web.TwoFactor.InputModels;

namespace PJMS.AuthService.Web.TwoFactor.ViewModels;

/// <summary>
/// Класс модели представления для настройки двухфакторной аутентификации.
/// </summary>
public class SetupTwoFactorViewModel : SetupTwoFactorInputModel
{
    /// <summary>
    /// Формат URI для аутентификатора.
    /// </summary>
    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    /// <summary>
    /// URL QR-кода.
    /// </summary>
    public string QrCodeUrl { get; }

    /// <summary>
    /// Конструктор класса SetupTwoFactorViewModel.
    /// </summary>
    /// <param name="authenticatorKey">Ключ аутентификатора.</param>
    /// <param name="name">Имя пользователя.</param>
    /// <param name="projectName">Название проекта.</param>
    public SetupTwoFactorViewModel(string authenticatorKey, string name, string projectName)
    {
        AuthenticatorKey = FormatKey(authenticatorKey);
        QrCodeUrl = GenerateQrCodeUrl(name, projectName, authenticatorKey);
    }

    /// <summary>
    /// Форматирует ключ.
    /// </summary>
    private static string FormatKey(string unformattedKey)
    {
        // Создание нового экземпляра StringBuilder для хранения результата
        var result = new StringBuilder();

        // Инициализация переменной currentPosition для отслеживания текущей позиции в строке
        var currentPosition = 0;

        // Цикл while для обработки строки unformattedKey поблочно по 4 символа
        while (currentPosition + 4 < unformattedKey.Length)
        {
            // Добавление следующих 4 символов из unformattedKey в результат и пробела
            result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');

            // Увеличение текущей позиции на 4
            currentPosition += 4;
        }

        // Добавление оставшихся символов из unformattedKey в результат, если есть
        if (currentPosition < unformattedKey.Length)
            result.Append(unformattedKey[currentPosition..]);

        // Преобразование результата в строку и приведение к нижнему регистру
        return result.ToString().ToLowerInvariant();
    }

    /// <summary>
    /// Генерирует URL QR-кода.
    /// </summary>
    private static string GenerateQrCodeUrl(string name, string projectName, string unformattedKey)
    {
        // Возвращает отформатированную строку URI аутентификатора, используя формат AuthenticatorUriFormat
        return string.Format(AuthenticatorUriFormat, HttpUtility.UrlEncode(projectName),
            HttpUtility.UrlEncode(name), unformattedKey);
    }
}