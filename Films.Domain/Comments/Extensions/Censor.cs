using System.Text.RegularExpressions;

namespace Films.Domain.Comments.Extensions;

public static class Censor
{
    // Статический массив запрещенных слов
    private static readonly string[] BadWords = new[]
    {
        "*бля*",
        "*хуй*",
        "*хуя*",
        "*хуе*",
        "*пизд*",
        "*пидор*",
        "*педик*",
        "*письк*",
        "*жопа",
        "*писюн*",
        "*ебал*",
        "*ебу*",
        "*ебан*",
        "*ёбан*",
        "*ебат*",
        "*ебну*",
        "*ёбну*",
        "*ебит*",
        "*уеб*",
        "*уёб*",
        "*сперм*",
        "*залуп*",
        "*пёзд*",
        "*пезд*",
        "*трах*"
    }.Select(ToRegexPattern).ToArray();

    public static string CensorText(this string censoredText)
    {
        foreach (var censoredWord in BadWords)
        {
            censoredText = Regex.Replace(censoredText, censoredWord, StarCensoredMatch,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        return censoredText;
    }

    private static string StarCensoredMatch(Match m)
    {
        var word = m.Captures[0].Value;

        return new string('*', word.Length);
    }

    private static string ToRegexPattern(string wildcardSearch)
    {
        var regexPattern = Regex.Escape(wildcardSearch);

        regexPattern = regexPattern.Replace(@"\*", ".*?");
        regexPattern = regexPattern.Replace(@"\?", ".");

        if (regexPattern.StartsWith(".*?"))
        {
            regexPattern = regexPattern[3..];
            regexPattern = @"(^\b)*?" + regexPattern;
        }

        regexPattern = @"\b" + regexPattern + @"\b";

        return regexPattern;
    }
}