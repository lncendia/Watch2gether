using Films.Domain.Abstractions;
using Films.Domain.Comments.Exceptions;
using Films.Domain.Films;

namespace Films.Domain.Comments;

/// <summary>
/// Класс Comment представляет сущность комментария и наследуется от AggregateRoot. 
/// Он содержит информацию о комментарии, относящемся к фильму.
/// </summary>
public class Comment : AggregateRoot
{
    // Статический массив запрещенных слов
    private static readonly string[] BadWords =
    {
        // Здесь перечислены запрещенные слова
        "бля", "хуй", "хуя", "хуе", "пизд", "пидор", "педик", "письк", "жопа", "писюн", "сука", "вагина", "влагалище",
        "сучий", "ебал", "ебу", "ебан", "ёбан", "ебат", "ебну", "ёбну", "ебит", "уеб", "уёб", "сперм", "залуп", "анал",
        "анус", "сучи", "гандон", "гнида", "говн", "сосать", "соси", "жопе", "пёзд", "пезд", "трах"
    };

    /// <summary>
    /// Конструктор класса Comment, создающий новый комментарий.
    /// </summary>
    /// <param name="film">Экземпляр фильма, к которому относится комментарий.</param>
    /// <param name="userId">Идентификатор пользователя, создавшего комментарий.</param>
    /// <param name="text">Текст комментария.</param>
    /// <exception cref="TextLengthException">Вызывается, если текст комментария пустой или слишком длинный.</exception>
    public Comment(Film film, Guid userId, string text)
    {
        // Запоминаем идентификатор фильма
        FilmId = film.Id;
        // Запоминаем идентификатор пользователя
        UserId = userId;
        // Проверяем текст комментария на пустоту и длину
        if (string.IsNullOrEmpty(text) || text.Length > 1000) throw new TextLengthException();

        // Фильтрация запрещенных слов в тексте комментария
        foreach (var badWord in BadWords)
        {
            // Поиск индекса запрещенного слова в тексте, не учитывая регистр символов
            var index = text.IndexOf(badWord, StringComparison.OrdinalIgnoreCase);
            while (index != -1) // Пока в тексте есть запрещенные слова
            {
                // Замена запрещенного слова звёздочками
                text = text.Remove(index, badWord.Length).Insert(index, new string('*', badWord.Length));
                // Поиск следующего индекса запрещенного слова в тексте
                index = text.IndexOf(badWord, index, StringComparison.OrdinalIgnoreCase);
            }
        }

        // Сохраняем отфильтрованный текст комментария
        Text = text;
    }

    /// <summary>
    /// Идентификатор фильма, к которому относится комментарий.
    /// </summary>
    public Guid FilmId { get; }

    /// <summary>
    /// Идентификатор пользователя, создавшего комментарий.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Текст комментария.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Время создания комментария.
    /// </summary>
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}
