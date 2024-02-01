using Films.Application.Abstractions.Commands.Comments;
using Films.Application.Abstractions.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Comments.Entities;
using Films.Domain.Films.Entities;

namespace Films.Application.Services.Commands.Comments;

public class AddCommentCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : IRequestHandler<AddCommentCommand, Guid>
{
    public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();
        var comment = new Comment(await GetFilmAsync(request.FilmId), request.UserId, request.Text);
        await unitOfWork.CommentRepository.Value.AddAsync(comment);
        await unitOfWork.SaveChangesAsync();
        return comment.Id;
    }

    /// <summary>
    /// Асинхронно получает фильм из кэша или базы данных по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор фильма.</param>
    /// <returns>Объект фильма.</returns>
    private async Task<Film> GetFilmAsync(Guid id)
    {
        // Проверяем, есть ли фильм в кэше 
        if (memoryCache.TryGetValue(id, out Film? film)) return film!;
        
        // Если фильм не найден в кэше, получаем его из репозитория
        film = await unitOfWork.FilmRepository.Value.GetAsync(id);

        // Если фильм не найден, выбрасываем исключение 
        if (film == null) throw new FilmNotFoundException();

        // Добавляем фильм в кэш с временем жизни 5 минут 
        memoryCache.Set(id, film, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

        // Возвращаем найденный фильм 
        return film;
    }
}