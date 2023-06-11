using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Film.Specifications.Visitor;

public interface IFilmSpecificationVisitor : ISpecificationVisitor<IFilmSpecificationVisitor, Film.Entities.Film>
{
    void Visit(FilmByGenreSpecification specification);
    void Visit(FilmByActorSpecification specification);
    void Visit(FilmByDirectorSpecification specification);
    void Visit(FilmByScreenWriterSpecification specification);
    void Visit(FilmByTypeSpecification specification);
    void Visit(FilmByNameSpecification specification);
    void Visit(FilmByYearsSpecification specification);
    void Visit(FilmByCountrySpecification specification);
    void Visit(FilmByIdSpecification specification);
    void Visit(FilmByIdsSpecification specification);
}