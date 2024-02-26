using Films.Domain.Abstractions;
using Films.Domain.Extensions;
using Films.Domain.Films.Events;
using Films.Domain.Films.Exceptions;
using Films.Domain.Films.ValueObjects;
using Films.Domain.Ratings;

namespace Films.Domain.Films;

/// <summary>
/// Класс, представляющий фильм.
/// </summary>
public class Film : AggregateRoot
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Film"/>.
    /// </summary>
    /// <param name="isSerial">Тип фильма (Фильм или Сериал).</param>
    /// <param name="countSeasons">Количество сезонов (только для сериалов).</param>
    /// <param name="countEpisodes">Количество эпизодов (только для сериалов).</param>
    /// <exception cref="SerialException">Выбрасывается, если тип фильма - сериал, но количество сезонов или эпизодов не указано.</exception>
    public Film(bool isSerial, int? countSeasons = null, int? countEpisodes = null)
    {
        IsSerial = isSerial;
        if (isSerial)
        {
            if (countSeasons == null || countEpisodes == null) throw new SerialException();
            CountSeasons = countSeasons;
            CountEpisodes = countEpisodes;
        }

        AddDomainEvent(new NewFilmDomainEvent(this));
    }

    #region Info

    /// <summary> 
    /// Заголовок фильма. 
    /// </summary> 
    private readonly string _title = null!;

    /// <summary> 
    /// Описание фильма. 
    /// </summary> 
    private string _description = null!;

    /// <summary> 
    /// Краткое описание фильма. 
    /// </summary> 
    private string? _shortDescription;

    /// <summary> 
    /// Заголовок фильма. 
    /// </summary> 
    public required string Title
    {
        get => _title;
        init
        {
            if (string.IsNullOrEmpty(value) || value.Length > 200) throw new FilmTitleLengthException();
            _title = value.GetUpper();
        }
    }

    /// <summary> 
    /// Описание фильма. 
    /// </summary> 
    public required string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 1500) throw new FilmDescriptionLengthException();
            _description = value.GetUpper();
        }
    }

    /// <summary> 
    /// Краткое описание фильма. 
    /// </summary> 
    public string ShortDescription
    {
        get
        {
            if (!string.IsNullOrEmpty(_shortDescription)) return _shortDescription;
            if (_description.Length < 100) return _description;
            return _description[..100] + "...";
        }
        set
        {
            if (value == string.Empty || value.Length > 500) throw new FilmShortDescriptionLengthException();
            _shortDescription = value.GetUpper();
        }
    }

    /// <summary> 
    /// Год выпуска фильма. 
    /// </summary> 
    public required int Year { get; init; }

    /// <summary> 
    /// URL постера фильма. 
    /// </summary> 
    public required Uri PosterUrl { get; set; }

    #endregion

    #region Collections

    /// <summary> 
    /// Список стран, связанных с фильмом. 
    /// </summary> 
    private readonly string[] _countries = null!;

    /// <summary> 
    /// Список режиссеров фильма. 
    /// </summary> 
    private readonly string[] _directors = null!;

    /// <summary> 
    /// Список сценаристов фильма. 
    /// </summary> 
    private readonly string[] _screenwriters = null!;

    /// <summary> 
    /// Список актеров фильма. 
    /// </summary> 
    private readonly Actor[] _actors = null!;

    /// <summary> 
    /// Список жанров фильма. 
    /// </summary> 
    private readonly string[] _genres = null!;

    /// <summary> 
    /// Жанры фильма. 
    /// </summary> 
    public required IReadOnlyCollection<string> Genres
    {
        get => _genres;
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _genres = value.Select(s => s.GetUpper()).ToArray();
        }
    }

    /// <summary> 
    /// Страны фильма. 
    /// </summary> 
    public required IReadOnlyCollection<string> Countries
    {
        get => _countries;
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _countries = value.Select(s => s.GetUpper()).ToArray();
        }
    }

    /// <summary> 
    /// Режиссеры фильма. 
    /// </summary> 
    public required IReadOnlyCollection<string> Directors
    {
        get => _directors;
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _directors = value.Select(s => s.GetUpper()).ToArray();
        }
    }

    /// <summary> 
    /// Актеры фильма. 
    /// </summary> 
    public required IReadOnlyCollection<Actor> Actors
    {
        get => _actors;
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _actors = value.ToArray();
        }
    }

    /// <summary> 
    /// Сценаристы фильма. 
    /// </summary> 
    public required IReadOnlyCollection<string> Screenwriters
    {
        get => _screenwriters;
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _screenwriters = value.Select(s => s.GetUpper()).ToArray();
        }
    }

    #endregion

    #region Rating

    /// <summary> 
    /// Пользовательский рейтинг фильма. 
    /// </summary> 
    public double UserRating { get; private set; }

    /// <summary> 
    /// Количество пользовательских оценок фильма. 
    /// </summary> 
    public int UserRatingsCount { get; private set; }

    /// <summary> 
    /// Добавляет рейтинг пользователя и обновляет пользовательский рейтинг. 
    /// </summary> 
    /// <param name="rating">Новый рейтинг пользователя.</param> 
    /// <param name="oldRating">Старый рейтинг пользователя (если есть).</param> 
    public void AddRating(Rating rating, Rating? oldRating)
    {
        var allScore = UserRating * UserRatingsCount;
        if (oldRating == null) UserRatingsCount++;
        else allScore -= oldRating.Score;
        UserRating = (allScore + rating.Score) / UserRatingsCount;
    }

    /// <summary> 
    /// Рейтинг фильма на КиноПоиске. 
    /// </summary> 
    private double? _ratingKp;

    /// <summary> 
    /// Рейтинг фильма на КиноПоиске. 
    /// </summary> 
    public double? RatingKp
    {
        get => _ratingKp;
        set
        {
            if (value is < 0 or > 10)
                throw new ArgumentOutOfRangeException(nameof(value), "Рейтинг должен быть от 0 до 10");
            _ratingKp = value;
        }
    }

    /// <summary> 
    /// Рейтинг фильма на IMDb. 
    /// </summary> 
    private double? _ratingImdb;

    /// <summary> 
    /// Рейтинг фильма на IMDb. 
    /// </summary> 
    public double? RatingImdb
    {
        get => _ratingImdb;
        set
        {
            if (value is < 0 or > 10)
                throw new ArgumentOutOfRangeException(nameof(value), "Рейтинг должен быть от 0 до 10");
            _ratingImdb = value;
        }
    }

    #endregion

    #region Type

    /// <summary>
    /// Возвращает тип фильма (сериал или фильм).
    /// </summary>
    public bool IsSerial { get; }

    /// <summary>
    /// Возвращает количество сезонов.
    /// </summary>
    public int? CountSeasons { get; private set; }

    /// <summary>
    /// Возвращает количество эпизодов.
    /// </summary>
    public int? CountEpisodes { get; private set; }

    /// <summary>
    /// Обновляет информацию о сериале - количество сезонов и эпизодов.
    /// </summary>
    /// <param name="countSeasons">Количество сезонов.</param>
    /// <param name="countEpisodes">Количество эпизодов.</param>
    public void UpdateSeriesInfo(int countSeasons, int countEpisodes)
    {
        // Проверяет, является ли фильм сериалом.
        if (!IsSerial) throw new NotSerialException();

        // Устанавливает количество сезонов и эпизодов.
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
    }

    #endregion

    #region Cdn

    /// <summary>
    /// Коллекция CDN (Content Delivery Network) для фильма.
    /// </summary>
    private readonly List<Cdn> _cdnList = [];

    /// <summary>
    /// Возвращает коллекцию CDN в виде доступного только для чтения списка.
    /// </summary>
    public required IReadOnlyCollection<Cdn> CdnList
    {
        get => _cdnList;
        init
        {
            // Устанавливает новую коллекцию CDN и проверяет наличие дубликатов.
            if (value.Count == 0) throw new EmptyCdnsCollectionException();
            if (_cdnList.GroupBy(x => x.Name).Any(x => x.Count() > 1)) throw new DuplicateCdnException();
            _cdnList = value.ToList();
        }
    }

    /// <summary>
    /// Добавляет новую CDN или изменяет существующую.
    /// </summary>
    public void AddOrChangeCdn(Cdn cdn)
    {
        // Удаляет существующую CDN (если таковая имеется) и добавляет новую.
        _cdnList.RemoveAll(x => x.Name == cdn.Name);
        _cdnList.Add(cdn);
    }

    #endregion
}