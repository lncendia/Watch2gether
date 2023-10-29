using Overoom.Application.Abstractions.Films.DTOs;
using Overoom.Application.Abstractions.Films.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Enums;
using Overoom.Domain.Films.Ordering;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists.Entities;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Application.Services.FilmsCatalog;

/// <summary> 
/// Класс, отвечающий за управление фильмами. 
/// </summary> 
public class FilmsManager : IFilmsManager
{
    /// <summary> 
    /// Экземпляр интерфейса IUnitOfWork, представляющий единицу работы для доступа к данным. 
    /// </summary> 
    private readonly IUnitOfWork _unitOfWork;
    
    /// <summary> 
    /// Экземпляр интерфейса IFilmsMapper, используемый для отображения объектов фильма.
    /// </summary> 
    private readonly IFilmsMapper _mapper;

    public FilmsManager(IUnitOfWork unitOfWork, IFilmsMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary> 
    /// Возвращает список популярных фильмов. 
    /// </summary> 
    /// <returns>Список DTO фильмов.</returns> 
    public async Task<List<FilmDto>> PopularFilmsAsync()
    {
        // Определяем порядок сортировки по дате и рейтингу пользователя 
        var date = new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByDate()); 
        var rating = new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByUserRatingCount()); 
        var order = new ThenByOrder<Film, IFilmSortingVisitor>(date, rating); 
 
        // Получаем список фильмов из репозитория с применением сортировки и ограничением на количество 
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(null, order, 0, 15); 
 
        // Преобразуем фильмы в список DTO фильмов 
        return films.Select(_mapper.Map).ToList(); 
    }

    /// <summary> 
    /// Возвращает список лучших фильмов. 
    /// </summary> 
    /// <returns>Список DTO фильмов.</returns> 
    public async Task<List<FilmDto>> BestFilmsAsync()
    {
        // Определяем порядок сортировки по дате и пользовательскому рейтингу 
        var date = new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByDate()); 
        var rating = new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByUserRating()); 
        var order = new ThenByOrder<Film, IFilmSortingVisitor>(date, rating); 
 
        // Получаем список фильмов из репозитория с применением сортировки и ограничением на количество 
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(null, order, 0, 15); 
 
        // Преобразуем фильмы в список DTO фильмов 
        return films.Select(_mapper.Map).ToList(); 
    }

    /// <summary> 
    /// Поиск фильмов с заданными параметрами. 
    /// </summary> 
    /// <param name="searchQuery">Параметры поиска.</param> 
    /// <returns>Список DTO фильмов.</returns>
    public async Task<List<FilmDto>> FindAsync(FilmSearchQuery searchQuery)
    {
        ISpecification<Film, IFilmSpecificationVisitor>? specification = null;

        // Добавляем спецификации в соответствии с заданными параметрами поиска 
        if (!string.IsNullOrEmpty(searchQuery.Query))
            specification = AddToSpecification(specification, FilmByTitle(searchQuery.Query));

        if (!string.IsNullOrEmpty(searchQuery.Genre))
            specification = AddToSpecification(specification, FilmByGenre(searchQuery.Genre));

        if (!string.IsNullOrEmpty(searchQuery.Person))
            specification = AddToSpecification(specification, FilmByPerson(searchQuery.Person));

        if (!string.IsNullOrEmpty(searchQuery.Country))
            specification = AddToSpecification(specification, FilmByCountry(searchQuery.Country));

        if (searchQuery.Type != null)
            specification = AddToSpecification(specification, FilmByType(searchQuery.Type.Value));

        if (searchQuery.Playlist != null)
        {
            var playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(searchQuery.Playlist.Value);
            if (playlist != null) specification = AddToSpecification(specification, FilmByPlaylist(playlist));
        }

        // Определяем порядок сортировки по дате 
        IOrderBy<Film, IFilmSortingVisitor> orderBy =
            new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByDate());

        // Получаем список фильмов из репозитория с применением спецификаций, сортировки и ограничением на количество 
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(specification, orderBy,
            (searchQuery.Page - 1) * 10, 10);
        
        // Преобразуем фильмы в список DTO фильмов 
        return films.Select(_mapper.Map).ToList();
    }


    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByPerson(string person)
    {
        ISpecification<Film, IFilmSpecificationVisitor>
            spec = new FilmByActorSpecification(person);

        spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
            new FilmByDirectorSpecification(person));

        spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
            new FilmByScreenWriterSpecification(person));
        return spec;
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByGenre(string genre)
    {
        return new FilmByGenreSpecification(genre);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByTitle(string title)
    {
        return new FilmByNameSpecification(title);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByCountry(string country)
    {
        return new FilmByCountrySpecification(country);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByType(FilmType type)
    {
        return new FilmByTypeSpecification(type);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByPlaylist(Playlist playlist)
    {
        return new FilmByIdsSpecification(playlist.Films);
    }

    private static ISpecification<T, TV> AddToSpecification<T, TV>(
        ISpecification<T, TV>? baseSpec, ISpecification<T, TV> newSpec) where TV : ISpecificationVisitor<TV, T>
    {
        return baseSpec == null
            ? newSpec
            : new AndSpecification<T, TV>(baseSpec, newSpec);
    }
}