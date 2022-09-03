using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Films.Specifications.Visitor;

public interface IFilmSpecificationVisitor : ISpecificationVisitor<IFilmSpecificationVisitor, Film>
{
    void Visit(FilmFromGenreSpecification specification);
    void Visit(FilmFromActorSpecification specification);
    void Visit(FilmFromDirectorSpecification specification);
    void Visit(FilmFromScreenWriterSpecification specification);
    void Visit(FilmFromTypeSpecification specification);
    void Visit(FilmFromNameSpecification specification);
    void Visit(FilmFromYearsSpecification specification);
    void Visit(FilmFromCountrySpecification specification);
    void Visit(FilmFromIdsSpecification specification);
}