using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.DTO.StartPage;
using Overoom.Application.Abstractions.Interfaces.StartPage;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Comments;
using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Films;
using Overoom.Domain.Films.Ordering;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Ordering;
using Overoom.Domain.Rooms.FilmRoom;
using Overoom.Domain.Rooms.FilmRoom.Ordering;
using Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;
using Overoom.Domain.Rooms.YoutubeRoom;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users;
using Overoom.Domain.Users.Specifications;
using Overoom.Domain.Users.Specifications.Visitor;
using Type = Overoom.Application.Abstractions.DTO.StartPage.Type;

namespace Overoom.Application.Services.Services.StartPage;

public class StartPageService : IStartPageService
{
    private readonly IUnitOfWork _unitOfWork;

    public StartPageService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<StartInfoDto> GetStartInfoAsync()
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
            new DescendingOrder<Film, IFilmSortingVisitor>(new OrderByDate()), take: 10);
        return films.Select(x => new FilmStartPageDto(x.Name, x.PosterFileName, x.Id, x.FilmCollections.Genres));
    }

    private async Task<IEnumerable<CommentStartPageDto>> GetCommentsAsync()
    {
        var comments = await _unitOfWork.CommentRepository.Value.FindAsync(null,
            new DescendingOrder<Comment, ICommentSortingVisitor>(new Domain.Comments.Ordering.OrderByDate()), take: 20);

        ISpecification<User, IUserSpecificationVisitor> spec = new UserByIdSpecification(comments[0].UserId);
        for (var i = 1; i < comments.Count; i++)
        {
            spec = new OrSpecification<User, IUserSpecificationVisitor>(spec,
                new UserByIdSpecification(comments[i].UserId));
        }

        var users = await _unitOfWork.UserRepository.Value.FindAsync(spec);

        return from comment in comments
            let user = users.FirstOrDefault(x => x.Id == comment.UserId)
            select new CommentStartPageDto(user?.Name ?? "Удаленный пользователь", comment.Text, comment.CreatedAt,
                comment.FilmId, user?.AvatarFileName ?? ApplicationConstants.DefaultAvatar);
    }

    private async Task<IEnumerable<RoomStartPageDto>> GetRoomsAsync()
    {
        var frooms = _unitOfWork.FilmRoomRepository.Value.FindAsync(null,
            new DescendingOrder<FilmRoom, IFilmRoomSortingVisitor>(new OrderByLastActivityDate()), take: 5);
        var yrooms = _unitOfWork.YoutubeRoomRepository.Value.FindAsync(null,
            new DescendingOrder<YoutubeRoom, IYoutubeRoomSortingVisitor>(
                new Domain.Rooms.YoutubeRoom.Ordering.OrderByLastActivityDate()), take: 5);

        await Task.WhenAll(frooms, yrooms);

        ISpecification<Film, IFilmSpecificationVisitor> spec = new FilmByIdSpecification(frooms.Result[0].FilmId);
        for (var i = 1; i < frooms.Result.Count; i++)
        {
            spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
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