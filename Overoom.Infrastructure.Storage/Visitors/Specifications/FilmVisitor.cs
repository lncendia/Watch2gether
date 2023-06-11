using System.Linq.Expressions;
using Overoom.Domain.Film.Entities;
using Overoom.Domain.Film.Specifications;
using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.Storage.Models.Film;

namespace Overoom.Infrastructure.Storage.Visitors.Specifications;

public class FilmVisitor : BaseVisitor<FilmModel, IFilmSpecificationVisitor, Film>, IFilmSpecificationVisitor
{
    protected override Expression<Func<FilmModel, bool>> ConvertSpecToExpression(
        ISpecification<Film, IFilmSpecificationVisitor> spec)
    {
        var visitor = new FilmVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(FilmByGenreSpecification specification) =>
        Expr = model =>
            model.Genres.Any(g => g.Name.ToLower() == specification.Genre.ToLower());

    public void Visit(FilmByActorSpecification specification) =>
        Expr = model =>
            model.Actors.Any(g => g.Name.ToLower() == specification.Actor.ToLower());

    public void Visit(FilmByDirectorSpecification specification) =>
        Expr = model =>
            model.Directors.Any(g => g.Name.ToLower() == specification.Director.ToLower());

    public void Visit(FilmByScreenWriterSpecification specification) =>
        Expr = model =>
            model.ScreenWriters.Any(g => g.Name.ToLower() == specification.ScreenWriter.ToLower());

    public void Visit(FilmByTypeSpecification specification) => Expr = model => model.Type == specification.Type;

    public void Visit(FilmByNameSpecification specification) =>
        Expr = model => model.Name.Contains(specification.Name);

    public void Visit(FilmByYearsSpecification specification) => Expr = model =>
        model.Year <= specification.MaxYear && model.Year >= specification.MinYear;

    public void Visit(FilmByCountrySpecification specification) =>
        Expr = model =>
            model.Countries.Any(g => g.Name == specification.Country);

    public void Visit(FilmByIdSpecification specification) => Expr = model => specification.Id == model.Id;

    public void Visit(FilmByIdsSpecification specification) =>
        Expr = model => specification.Ids.Any(x => x == model.Id);
}