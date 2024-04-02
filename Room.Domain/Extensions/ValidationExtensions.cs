namespace Room.Domain.Extensions;

internal static class ValidationExtensions
{
    public static string ValidateLength(this string value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > maxLength)
            throw new ArgumentException($"Value should not be null, empty or longer than {maxLength} characters.");

        return value;
    }
}