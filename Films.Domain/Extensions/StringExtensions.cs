namespace Films.Domain.Extensions;

public static class StringExtensions
{
    public static string GetUpper(this string s) => $"{char.ToUpperInvariant(s[0])}{s[1..]}";
}