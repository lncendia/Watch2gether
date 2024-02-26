using PJMS.AuthService.Abstractions.Enums;

namespace PJMS.AuthService.Abstractions.Accessories;

/// <summary>
/// Класс вспомогательных методов для форматирования данных
/// </summary>
public static class LocalizationExtensions
{
    /// <summary>
    /// Русский язык
    /// </summary>
    public const string Ru = "ru";

    /// <summary>
    /// Английский язык
    /// </summary>
    public const string En = "en";

    /// <summary>
    /// Испанский язык
    /// </summary>
    public const string Es = "es";

    /// <summary>
    /// Португальский язык
    /// </summary>
    public const string Pt = "pt";

    /// <summary>
    /// Немецкий язык
    /// </summary>
    public const string De = "de";

    /// <summary>
    /// Китайский язык
    /// </summary>
    public const string Zh = "zh";

    /// <summary>
    /// Японский язык
    /// </summary>
    public const string Ja = "ja";

    /// <summary>
    /// Турецкий язык
    /// </summary>
    public const string Tr = "tr";

    /// <summary>
    /// Французский язык
    /// </summary>
    public const string Fr = "fr";

    /// <summary>
    /// Итальянский язык
    /// </summary>
    public const string It = "it";

    /// <summary>
    /// Литовский язык
    /// </summary>
    public const string Lt = "lt";

    /// <summary>
    /// Украинский язык
    /// </summary>
    public const string Uk = "uk";

    /// <summary>
    /// Польский язык
    /// </summary>
    public const string Pl = "pl";

    /// <summary>
    /// Румынский язык
    /// </summary>
    public const string Ro = "ro";

    /// <summary>
    /// Нидерландский язык
    /// </summary>
    public const string Nl = "nl";

    /// <summary>
    /// Венгерский язык
    /// </summary>
    public const string Hu = "hu";

    /// <summary>
    /// Греческий язык
    /// </summary>
    public const string El = "el";

    /// <summary>
    /// Чешский язык
    /// </summary>
    public const string Cs = "cs";

    /// <summary>
    /// Шведский язык
    /// </summary>
    public const string Sv = "sv";

    /// <summary>
    /// Болгарский язык
    /// </summary>
    public const string Bg = "bg";

    /// <summary>
    /// Финский язык
    /// </summary>
    public const string Fi = "fi";

    /// <summary>
    /// Сербский язык
    /// </summary>
    public const string Sr = "sr";

    /// <summary>
    /// Корейский язык
    /// </summary>
    public const string Ko = "ko";

    /// <summary>
    /// Азербайджанский язык
    /// </summary>
    public const string Az = "az";

    /// <summary>
    /// Казахский язык
    /// </summary>
    public const string Kk = "kk";
    
    /// <summary>
    /// Белорусский язык
    /// </summary>
    public const string Be = "be";
    
    /// <summary>
    /// Метод - расширение отдает локализацию как строку
    /// </summary>
    /// <param name="localization">Локализация</param>
    /// <returns>Локализация как строка</returns>
    public static string GetLocalizationAsString(this Localization localization)
    {
        //проверяем локализацию и отдаем ее в нижнем регистре
        return localization == 0 ? En : localization.ToString().ToLower();
    }

    /// <summary>
    /// Метод - расширение отдает локализацию из строки
    /// </summary>
    /// <param name="localization">Локализация</param>
    /// <returns>Локализация как строка</returns>
    public static Localization GetLocalization(this string? localization)
    {
        //проверяем входящие данные
        if (localization == null) return Localization.En;

        //смотрим локализацию в нижнем регистре и отдаем значение из enum
        return localization.ToLower() switch
        {
            Ru => Localization.Ru,
            En => Localization.En,
            Es => Localization.Es,
            Pt => Localization.Pt,
            De => Localization.De,
            Zh => Localization.Zh,
            Ja => Localization.Ja,
            Tr => Localization.Tr,
            Fr => Localization.Fr,
            It => Localization.It,
            Lt => Localization.Lt,
            Uk => Localization.Uk,
            Pl => Localization.Pl,
            Ro => Localization.Ro,
            Nl => Localization.Nl,
            Hu => Localization.Hu,
            El => Localization.El,
            Cs => Localization.Cs,
            Sv => Localization.Sv,
            Bg => Localization.Bg,
            Fi => Localization.Fi,
            Sr => Localization.Sr,
            Ko => Localization.Ko,
            Az => Localization.Az,
            Kk => Localization.Kk,
            Be => Localization.Be,
            _ => Localization.En
        };
    }
    
    /// <summary>
    /// Метод - расширение отдает локализацию из строки
    /// </summary>
    /// <param name="localization">Локализация</param>
    /// <returns>Локализация как строка</returns>
    public static string GetLocalizationString(this Localization localization)
    {

        //смотрим локализацию в нижнем регистре и отдаем значение из enum
        return localization.ToString().ToLower();
    }
}