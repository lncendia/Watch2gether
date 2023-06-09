using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.StartPage.DTOs;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Comment.Ordering.Visitor;
using Overoom.Domain.Film.Ordering;
using Overoom.Domain.Film.Ordering.Visitor;
using Overoom.Domain.Film.Specifications;
using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Ordering;
using Overoom.Domain.Room.FilmRoom.Entities;
using Overoom.Domain.Room.FilmRoom.Ordering;
using Overoom.Domain.Room.FilmRoom.Ordering.Visitor;
using Overoom.Domain.Room.YoutubeRoom.Entities;
using Overoom.Domain.Room.YoutubeRoom.Ordering.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.User.Specifications;
using Overoom.Domain.User.Specifications.Visitor;
using Type = Overoom.Application.Abstractions.StartPage.DTOs.Type;

namespace Overoom.Application.Services.StartPage;

public class StartPageService : IStartPageService
{
    private readonly IUnitOfWork _unitOfWork;

    public StartPageService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<StartInfoDto> GetAsync()
    {
        var t1 = GetFilmsAsync();
        var t2 = GetCommentsAsync();
        var t3 = GetRoomsAsync();
        await Task.WhenAll(t1, t2, t3);
        return new StartInfoDto(t2.Result, t1.Result, t3.Result);
    }

    private async Task<IEnumerable<FilmStartPageDto>> GetFilmsAsync()
    {
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(null,
            new DescendingOrder<Domain.Film.Entities.Film, IFilmSortingVisitor>(new OrderByDate()), take: 10);
        return films.Select(x => new FilmStartPageDto(x.Name, x.PosterUri, x.Id, x.FilmTags.Genres));
    }

    private async Task<IEnumerable<CommentStartPageDto>> GetCommentsAsync()
    {
        var comments = await _unitOfWork.CommentRepository.Value.FindAsync(null,
            new DescendingOrder<Domain.Comment.Entities.Comment, ICommentSortingVisitor>(new Domain.Comment.Ordering.OrderByDate()), take: 20);

        ISpecification<Domain.User.Entities.User, IUserSpecificationVisitor> spec = new UserByIdSpecification(comments[0].UserId);
        for (var i = 1; i < comments.Count; i++)
        {
            spec = new OrSpecification<Domain.User.Entities.User, IUserSpecificationVisitor>(spec,
                new UserByIdSpecification(comments[i].UserId));
        }

        var users = await _unitOfWork.UserRepository.Value.FindAsync(spec);

        return from comment in comments
            let user = users.FirstOrDefault(x => x.Id == comment.UserId)
            select new CommentStartPageDto(user?.Name ?? "Удаленный пользователь", comment.Text, comment.CreatedAt,
                comment.FilmId, user?.AvatarUri ?? ApplicationConstants.DefaultAvatar);
    }

    private async Task<IEnumerable<RoomStartPageDto>> GetRoomsAsync()
    {
        var frooms = _unitOfWork.FilmRoomRepository.Value.FindAsync(null,
            new DescendingOrder<FilmRoom, IFilmRoomSortingVisitor>(new OrderByLastActivityDate()), take: 5);
        var yrooms = _unitOfWork.YoutubeRoomRepository.Value.FindAsync(null,
            new DescendingOrder<YoutubeRoom, IYoutubeRoomSortingVisitor>(
                new Domain.Room.YoutubeRoom.Ordering.OrderByLastActivityDate()), take: 5);

        await Task.WhenAll(frooms, yrooms);

        ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> spec = new FilmByIdSpecification(frooms.Result[0].FilmId);
        for (var i = 1; i < frooms.Result.Count; i++)
        {
            spec = new OrSpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor>(spec,
                new FilmByIdSpecification(frooms.Result[i].FilmId));
        }

        var films = await _unitOfWork.FilmRepository.Value.FindAsync(spec);

        var filmRooms = frooms.Result.Select(x => new RoomStartPageDto(x.Id, Type.Film, x.Viewers.Count,
            films.FirstOrDefault(y => y.Id == x.FilmId)?.Name ?? "Фильм не найден."));

        var youtubeRooms = yrooms.Result.Select(x =>
            new RoomStartPageDto(x.Id, Type.Youtube, x.Viewers.Count, x.Owner.CurrentVideoId));

        return filmRooms.Concat(youtubeRooms).OrderByDescending(x => x.CountUsers);
    }
}