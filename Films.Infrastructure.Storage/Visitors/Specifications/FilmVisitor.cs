using System.Linq.Expressions;
using Films.Domain.Films;
using Films.Domain.Films.Specifications;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Models.Film;

namespace Films.Infrastructure.Storage.Visitors.Specifications;

public class FilmVisitor : BaseVisitor<FilmModel, IFilmSpecificationVisitor, Film>, IFilmSpecificationVisitor
{
    protected override Expression<Func<FilmModel, bool>> ConvertSpecToExpression(
        ISpecification<Film, IFilmSpecificationVisitor> spec)
    {
        var visitor = new FilmVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(FilmsByGenreSpecification specification) =>
        Expr = model => model.Genres.Any(g => g.Name == specification.Genre);

    public void Visit(FilmsByActorSpecification specification) =>
        Expr = model => model.Actors.Any(g => g.Person.Name.Contains(specification.Actor));

    public void Visit(FilmsByDirectorSpecification specification) =>
        Expr = model => model.Directors.Any(g => g.Name.Contains(specification.Director));

    public void Visit(FilmsByScreenWriterSpecification specification) =>
        Expr = model => model.Screenwriters.Any(g => g.Name.Contains(specification.ScreenWriter));

    public void Visit(FilmsByTypeSpecification specification) =>
        Expr = model => model.IsSerial == specification.IsSerial;

    public void Visit(FilmsByTitleSpecification specification) =>
        Expr = model => model.Title.Contains(specification.Title);

    public void Visit(FilmsByYearsSpecification specification) => Expr = model =>
        model.Year <= specification.MaxYear && model.Year >= specification.MinYear;

    public void Visit(FilmsByCountrySpecification specification) =>
        Expr = model => model.Countries.Any(g => g.Name == specification.Country);

    public void Visit(FilmsByIdsSpecification specification) =>
        Expr = model => specification.Ids.Any(x => x == model.Id);
}