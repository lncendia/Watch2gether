using System.Linq.Expressions;
using Overoom.Domain.Playlist.Entities;
using Overoom.Domain.Playlist.Specifications;
using Overoom.Domain.Playlist.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.Storage.Models.Playlist;

namespace Overoom.Infrastructure.Storage.Visitors.Specifications;

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
}