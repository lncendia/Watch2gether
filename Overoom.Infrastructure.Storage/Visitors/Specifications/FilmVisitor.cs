using System.Linq.Expressions;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Films.Specifications.Visitor;
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
            model.Genres.Any(g => g.NameNormalized == specification.Genre.ToUpper());

    public void Visit(FilmByActorSpecification specification) =>
        Expr = model =>
            model.Actors.Any(g => g.NameNormalized.Contains(specification.Actor.ToUpper()));

    public void Visit(FilmByDirectorSpecification specification) =>
        Expr = model =>
            model.Directors.Any(g => g.NameNormalized.Contains(specification.Director.ToUpper()));

    public void Visit(FilmByScreenWriterSpecification specification) =>
        Expr = model =>
            model.ScreenWriters.Any(g => g.NameNormalized.Contains(specification.ScreenWriter.ToUpper()));

    public void Visit(FilmByTypeSpecification specification) => Expr = model => model.Type == specification.Type;

    public void Visit(FilmByNameSpecification specification) =>
        Expr = model => model.NameNormalized.Contains(specification.Name.ToUpper());

    public void Visit(FilmByYearsSpecification specification) => Expr = model =>
        model.Year <= specification.MaxYear && model.Year >= specification.MinYear;

    public void Visit(FilmByCountrySpecification specification) =>
        Expr = model =>
            model.Countries.Any(g => g.NameNormalized == specification.Country.ToUpper());

    public void Visit(FilmByIdSpecification specification) => Expr = model => specification.Id == model.Id;

    public void Visit(FilmByIdsSpecification specification) =>
        Expr = model => specification.Ids.Any(x => x == model.Id);
}