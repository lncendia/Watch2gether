using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Films.Specifications.Visitor;

public interface IFilmSpecificationVisitor : ISpecificationVisitor<IFilmSpecificationVisitor, Film>
{
    void Visit(FilmsByGenreSpecification specification);
    void Visit(FilmsByActorSpecification specification);
    void Visit(FilmsByDirectorSpecification specification);
    void Visit(FilmsByScreenWriterSpecification specification);
    void Visit(FilmsByTypeSpecification specification);
    void Visit(FilmsByTitleSpecification specification);
    void Visit(FilmsByYearsSpecification specification);
    void Visit(FilmsByCountrySpecification specification);
    void Visit(FilmsByIdsSpecification specification);
}