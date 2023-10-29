using Microsoft.Extensions.Caching.Memory;
using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Application.Abstractions.Movie.DTOs;
using Overoom.Application.Abstractions.Movie.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Enums;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Specifications;
using Overoom.Domain.Ratings.Specifications.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Services.Films
{
    /// <summary>
    /// Представляет менеджер для фильмов.
    /// </summary>
    public class FilmManager : IFilmManager
    {
        /// <summary>
        /// Экземпляр интерфейса IUnitOfWork, который представляет единицу работы для доступа к данным. 
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        
        /// <summary>
        /// Экземпляр интерфейса IFilmMapper, который используется для отображения данных фильма. 
        /// </summary>
        private readonly IFilmMapper _mapper;
        
        /// <summary>
        /// Экземпляр интерфейса IMemoryCache, который используется для кэширования данных в памяти.
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FilmManager"/>.
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork для операций с базой данных.</param>
        /// <param name="filmMapper">Маппер фильмов.</param>
        /// <param name="memoryCache">Кэш в памяти.</param>
        public FilmManager(IUnitOfWork unitOfWork, IFilmMapper filmMapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = filmMapper;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Асинхронно получает фильм по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор фильма.</param>
        /// <param name="userId">Идентификатор пользователя (необязательно).</param>
        /// <returns>DTO фильма.</returns>
        public async Task<FilmDto> GetAsync(Guid id, Guid? userId)
        {
            // Получаем фильм по его идентификатору 
            var film = await GetFilmAsync(id);
            
            Rating? rating = null;
            User? user = null;
            
            // Если не указан идентификатор пользователя, возвращаем отображение фильма без рейтинга и пользователя 
            if (!userId.HasValue) return _mapper.Map(film, rating, user);
            
            // Получаем пользователя по его идентификатору 
            user = await _unitOfWork.UserRepository.Value.GetAsync(userId.Value);
            if (user == null) throw new UserNotFoundException();
            
            // Добавляем фильм в историю пользователя 
            user.AddFilmToHistory(id);
            // Обновляем данные пользователя в репозитории 
            await _unitOfWork.UserRepository.Value.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            
            // Создаем спецификации для получения рейтинга пользователя для данного фильма 
            var userSpec = new RatingByUserSpecification(userId.Value);
            var filmSpec = new RatingByFilmSpecification(id);
            
            // Получаем список рейтингов, удовлетворяющих спецификациям 
            var ratingList = await _unitOfWork.RatingRepository.Value.FindAsync(
                new AndSpecification<Rating, IRatingSpecificationVisitor>(userSpec, filmSpec));
           
            // Получаем первый рейтинг из списка 
            rating = ratingList.FirstOrDefault();
            
            // Возвращаем отображение фильма с рейтингом и пользователем 
            return _mapper.Map(film, rating, user);
        }

        /// <summary>
        /// Асинхронно переключает статус списка просмотра фильма для указанного пользователя.
        /// </summary>
        /// <param name="id">Идентификатор фильма.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        public async Task ToggleWatchlistAsync(Guid id, Guid userId)
        {
            // Получаем фильм по его идентификатору 
            await GetFilmAsync(id);
            
            // Получаем пользователя по его идентификатору 
            var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
            if (user == null) throw new UserNotFoundException();
            
            // Проверяем, находится ли фильм в списке просмотра пользователя 
            if (user.Watchlist.Any(x => x.FilmId == id)) 
                user.RemoveFilmFromWatchlist(id); // Если да, удаляем фильм из списка просмотра 
            else 
                user.AddFilmToWatchlist(id); // Если нет, добавляем фильм в список просмотра 
            
            // Обновляем данные пользователя в репозитории 
            await _unitOfWork.UserRepository.Value.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Асинхронно добавляет рейтинг для фильма от указанного пользователя.
        /// </summary>
        /// <param name="id">Идентификатор фильма.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="score">Оценка рейтинга.</param>
        /// <returns>DTO рейтинга.</returns>
        public async Task<RatingDto> AddRatingAsync(Guid id, Guid userId, double score)
        {
            // Получаем пользователя по его идентификатору 
            var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
            if (user == null) throw new UserNotFoundException();
            
            // Создаем новый рейтинг с указанным идентификатором фильма, идентификатором пользователя и оценкой 
            var rating = new Rating(id, user.Id, score);
            
            // Добавляем рейтинг в репозиторий 
            await _unitOfWork.RatingRepository.Value.AddAsync(rating);
            await _unitOfWork.SaveChangesAsync();
            
            // Получаем фильм по его идентификатору 
            var film = await GetFilmAsync(id);
            
            // Возвращаем DTO рейтинга фильма, содержащее среднюю оценку и количество оценок пользователей 
            return new RatingDto(film.UserRating, film.UserRatingsCount);
        }

        /// <summary>
        /// Асинхронно получает URI фильма по его идентификатору и типу CDN.
        /// </summary>
        /// <param name="id">Идентификатор фильма.</param>
        /// <param name="type">Тип CDN.</param>
        /// <returns>URI фильма.</returns>
        public async Task<Uri> GetFilmUriAsync(Guid id, CdnType type)
        {
            var film = await GetFilmAsync(id);
            var cdn = film.CdnList.FirstOrDefault(x => x.Type == type);
            if (cdn == null) throw new NotImplementedException();
            return cdn.Uri;
        }

        /// <summary>
        /// Асинхронно получает фильм из кэша или базы данных по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор фильма.</param>
        /// <returns>Объект фильма.</returns>
        private async Task<Domain.Films.Entities.Film> GetFilmAsync(Guid id)
        {
            // Проверяем, есть ли фильм в кэше 
            if (!_memoryCache.TryGetValue(id, out Domain.Films.Entities.Film? film))
            {
                // Если фильм не найден в кэше, получаем его из репозитория
                film = await _unitOfWork.FilmRepository.Value.GetAsync(id);
                
                // Если фильм не найден, выбрасываем исключение 
                if (film == null) throw new FilmNotFoundException();
                
                // Добавляем фильм в кэш с временем жизни 5 минут 
                _memoryCache.Set(id, film, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            else
            {
                // Если фильм в кэше равен null, выбрасываем исключение 
                if (film == null) throw new FilmNotFoundException();
            }
            
            // Возвращаем найденный фильм 
            return film;
        }
    }
}