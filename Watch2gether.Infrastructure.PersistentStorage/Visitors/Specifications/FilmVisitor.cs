using System.Linq.Expressions;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Films.Specifications;
using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Models.Films;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

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
            model.Genres.Any(g =>
                string.Equals(g.Name, specification.Genre, StringComparison.CurrentCultureIgnoreCase));

    public void Visit(FilmByActorSpecification specification) =>
        Expr = model =>
            model.Actors.Any(g =>
                string.Equals(g.Name, specification.Actor, StringComparison.CurrentCultureIgnoreCase));

    public void Visit(FilmByDirectorSpecification specification) =>
        Expr = model =>
            model.Directors.Any(g =>
                string.Equals(g.Name, specification.Director, StringComparison.CurrentCultureIgnoreCase));

    public void Visit(FilmByScreenWriterSpecification specification) =>
        Expr = model =>
            model.ScreenWriters.Any(g =>
                string.Equals(g.Name, specification.ScreenWriter, StringComparison.CurrentCultureIgnoreCase));

    public void Visit(FilmByTypeSpecification specification) => Expr = model => model.Type == specification.Type;

    public void Visit(FilmByNameSpecification specification) =>
        Expr = model => model.Name.Contains(specification.Name);

    public void Visit(FilmByYearsSpecification specification) => Expr = model =>
        model.Year <= specification.MaxYear && model.Year >= specification.MinYear;

    public void Visit(FilmByCountrySpecification specification) =>
        Expr = model =>
            model.Countries.Any(g =>
                string.Equals(g.Name, specification.Country, StringComparison.CurrentCultureIgnoreCase));

    public void Visit(FilmByIdSpecification specification) => Expr = model => specification.Id == model.Id;
}