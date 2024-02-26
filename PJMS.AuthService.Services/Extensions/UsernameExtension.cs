namespace PJMS.AuthService.Services.Extensions;

/// <summary>
/// Расширения для работы с именем пользователя.
/// </summary>
public static class UsernameExtension
{
    /// <summary>
    /// Метод расширения для работы с именем пользователя.
    /// </summary>
    /// <param name="username">Имя пользователя.</param>
    /// <param name="length">Длина, до которой надо обрезать строку</param>
    /// <returns>Обрезанную строку</returns>
    public static string CutTo(this string username, int length)
    {
        return username.Length > length ? username[..length] : username;
    }
}