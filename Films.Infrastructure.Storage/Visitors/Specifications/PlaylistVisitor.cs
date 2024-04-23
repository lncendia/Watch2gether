using System.Linq.Expressions;
using Films.Domain.Playlists;
using Films.Domain.Playlists.Specifications;
using Films.Domain.Playlists.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Models.Playlists;

namespace Films.Infrastructure.Storage.Visitors.Specifications;

public class PlaylistVisitor : BaseVisitor<PlaylistModel, IPlaylistSpecificationVisitor, Playlist>,
    IPlaylistSpecificationVisitor
{
    protected override Expression<Func<PlaylistModel, bool>> ConvertSpecToExpression(
        ISpecification<Playlist, IPlaylistSpecificationVisitor> spec)
    {
        var visitor = new PlaylistVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(PlaylistByFilmSpecification specification) =>
        Expr = model => model.Films.Any(x => x.FilmId == specification.Id);

    public void Visit(PlaylistByNameSpecification specification) =>
        Expr = model => model.Name.Contains(specification.Name);

    public void Visit(PlaylistByGenreSpecification specification) =>
        Expr = model => model.Genres.Any(x => x.Name == specification.Genre);
}