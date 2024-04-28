using System.Text.RegularExpressions;

namespace Films.Domain.Extensions;

internal static partial class ValidationExtensions
{
    private const string PhonePattern = @"^\+?\d{1,3}?[\s.-]?\(?\d{1,4}\)?[\s.-]?\d{1,4}[\s.-]?\d{1,4}[\s.-]?\d{1,4}$";
    private const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    
    public static string ValidateLength(this string value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > maxLength)
            throw new ArgumentException($"Value should not be null, empty or longer than {maxLength} characters.");

        return value;
    }

    public static string ValidateEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        }

        if (!EmailRegex().IsMatch(email))
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }

        return email;
    }
    
    public static string ValidatePhone(this string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Phone cannot be empty.", nameof(phone));
        }

        if (!PhoneRegex().IsMatch(phone))
        {
            throw new ArgumentException("Invalid phone format.", nameof(phone));
        }

        return phone;
    }
    
    [GeneratedRegex(PhonePattern)]
    private static partial Regex PhoneRegex();

    [GeneratedRegex(EmailPattern)]
    private static partial Regex EmailRegex();
}