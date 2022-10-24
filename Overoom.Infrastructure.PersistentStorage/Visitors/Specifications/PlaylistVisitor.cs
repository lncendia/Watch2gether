using System.Linq.Expressions;
using Overoom.Infrastructure.PersistentStorage.Models.Playlists;
using Overoom.Domain.Playlists;
using Overoom.Domain.Playlists.Specifications;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Specifications;

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
        Expr = model => model.FilmsList.Contains(specification.Id.ToString());

    public void Visit(PlaylistByNameSpecification specification) =>
        Expr = model => model.Name.Contains(specification.Name);
}