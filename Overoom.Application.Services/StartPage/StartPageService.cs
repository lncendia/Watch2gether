using Overoom.Application.Abstractions.StartPage.DTOs;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Comments.Ordering;
using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Ordering;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Specifications;
using Overoom.Domain.Users.Specifications.Visitor;
using Type = Overoom.Application.Abstractions.StartPage.DTOs.Type;

namespace Overoom.Application.Services.StartPage;

public class StartPageService : IStartPageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStartPageMapper _mapper;

    public StartPageService(IUnitOfWork unitOfWork, IStartPageMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<FilmStartPageDto>> GetFilmsAsync()
    {
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(null,
            new DescendingOrder<Domain.Films.Entities.Film, IFilmSortingVisitor>(new OrderByDate()), take: 10);
        return films.Select(_mapper.MapFilm).ToList();
    }

    public async Task<IReadOnlyCollection<CommentStartPageDto>> GetCommentsAsync()
    {
        var comments = await _unitOfWork.CommentRepository.Value.FindAsync(null,
            new DescendingOrder<Domain.Comments.Entities.Comment, ICommentSortingVisitor>(
                new Domain.Comments.Ordering.OrderByDate()), take: 20);

        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>? spec = null;

        foreach (var comment in comments.Where(x => x.UserId.HasValue))
        {
            spec = AddToSpecification(spec, new UserByIdSpecification(comment.UserId!.Value));
        }

        var users = await _unitOfWork.UserRepository.Value.FindAsync(spec);

        return comments.Select(x => _mapper.MapComment(x, users.FirstOrDefault(u => u.Id == x.UserId))).ToList();
    }

    public async Task<IReadOnlyCollection<RoomStartPageDto>> GetRoomsAsync()
    {
        var frooms = _unitOfWork.FilmRoomRepository.Value.FindAsync(null,
            new DescendingOrder<FilmRoom, IFilmRoomSortingVisitor>(new OrderByLastActivityDate()), take: 5);
        var yrooms = _unitOfWork.YoutubeRoomRepository.Value.FindAsync(null,
            new DescendingOrder<YoutubeRoom, IYoutubeRoomSortingVisitor>(
                new Domain.Rooms.YoutubeRoom.Ordering.OrderByLastActivityDate()), take: 5);

        await Task.WhenAll(frooms, yrooms);

        ISpecification<Domain.Films.Entities.Film, IFilmSpecificationVisitor> spec =
            new FilmByIdSpecification(frooms.Result[0].FilmId);
        for (var i = 1; i < frooms.Result.Count; i++)
        {
            spec = new OrSpecification<Domain.Films.Entities.Film, IFilmSpecificationVisitor>(spec,
                new FilmByIdSpecification(frooms.Result[i].FilmId));
        }

        var films = await _unitOfWork.FilmRepository.Value.FindAsync(spec);

        var filmRooms = frooms.Result.Select(x => new RoomStartPageDto(x.Id, Type.Film, x.Viewers.Count,
            films.FirstOrDefault(y => y.Id == x.FilmId)?.Name ?? "Фильм не найден."));

        var youtubeRooms = yrooms.Result.Select(x =>
            new RoomStartPageDto(x.Id, Type.Youtube, x.Viewers.Count, x.Owner.CurrentVideoId));

        return filmRooms.Concat(youtubeRooms).OrderByDescending(x => x.CountUsers);
    }


    private static ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor> AddToSpecification(
        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>? baseSpec,
        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor> newSpec)
    {
        return baseSpec == null
            ? newSpec
            : new AndSpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>(baseSpec, newSpec);
    }

    private static ISpecification<Domain.Films.Entities.Film, IFilmSpecificationVisitor> AddToSpecification(
        ISpecification<Domain.Films.Entities.Film, IFilmSpecificationVisitor>? baseSpec,
        ISpecification<Domain.Films.Entities.Film, IFilmSpecificationVisitor> newSpec)
    {
        return baseSpec == null
            ? newSpec
            : new AndSpecification<Domain.Films.Entities.Film, IFilmSpecificationVisitor>(baseSpec, newSpec);
    }
}