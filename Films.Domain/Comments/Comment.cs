using Films.Domain.Abstractions;
using Films.Domain.Comments.Extensions;
using Films.Domain.Extensions;
using Films.Domain.Films;
using Films.Domain.Users;

namespace Films.Domain.Comments;

/// <summary>
/// Класс Comment представляет сущность комментария и наследуется от AggregateRoot. 
/// Он содержит информацию о комментарии, относящемся к фильму.
/// </summary>
public class Comment : AggregateRoot
{
    private const int MaxTextLength = 100;

    /// <summary>
    /// Конструктор класса Comment, создающий новый комментарий.
    /// </summary>
    /// <param name="film">Экземпляр фильма, к которому относится комментарий.</param>
    /// <param name="user">Идентификатор пользователя, создавшего комментарий.</param>
    /// <param name="text">Текст комментария.</param>
    public Comment(Film film, User user, string text)
    {
        // Запоминаем идентификатор фильма
        FilmId = film.Id;

        // Запоминаем идентификатор пользователя
        UserId = user.Id;

        // Проверяем текст комментария на пустоту и длину

        // Сохраняем отфильтрованный текст комментария
        Text = text.ValidateLength(MaxTextLength).CensorText();
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