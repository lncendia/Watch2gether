using System.Linq.Expressions;
using Watch2gether.Domain.Playlists;
using Watch2gether.Domain.Playlists.Specifications;
using Watch2gether.Domain.Playlists.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Models.Playlists;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

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