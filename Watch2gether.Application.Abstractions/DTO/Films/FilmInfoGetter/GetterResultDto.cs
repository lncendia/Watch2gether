namespace Watch2gether.Application.Abstractions.DTO.Films.FilmInfoGetter;

public class GetterResultDto
{
    public GetterResultDto(List<FilmInfoDto> films, int totalCount, int lastPage, int page)
    {
        Films = films;
        TotalCount = totalCount;
        LastPage = lastPage;
        Page = page;
    }

    public List<FilmInfoDto> Films { get; }
    public int TotalCount { get; }
    public int LastPage { get; }
    public int Page { get; }
}