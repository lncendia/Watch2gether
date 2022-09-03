using System.Linq.Expressions;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Films.Specifications;
using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Models;

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

    public void Visit(FilmFromGenreSpecification specification) =>
        Expr = model => model.GenresList.ToLower().Contains(specification.Genre.ToLower());

    public void Visit(FilmFromActorSpecification specification) =>
        Expr = model => model.ActorsList.Contains(specification.Actor);

    public void Visit(FilmFromDirectorSpecification specification) =>
        Expr = model => model.DirectorsList.Contains(specification.Director);

    public void Visit(FilmFromScreenWriterSpecification specification) =>
        Expr = model => model.ScreenWritersList.Contains(specification.ScreenWriter);

    public void Visit(FilmFromTypeSpecification specification) => Expr = model => model.Type == specification.Type;

    public void Visit(FilmFromNameSpecification specification) =>
        Expr = model => model.Name.Contains(specification.Name);

    public void Visit(FilmFromYearsSpecification specification) => Expr = model =>
        model.Year <= specification.MaxYear && model.Year >= specification.MinYear;

    public void Visit(FilmFromCountrySpecification specification) =>
        Expr = model => model.CountriesList.Contains(specification.Country);

    public void Visit(FilmFromIdsSpecification specification) => Expr = model => specification.Ids.Contains(model.Id);
}