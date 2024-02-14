using Room.Domain.Abstractions;
using Room.Domain.Extensions;
using Room.Domain.Films.Enums;
using Room.Domain.Films.Exceptions;
using Room.Domain.Films.ValueObjects;

namespace Room.Domain.Films.Entities;

public class Film(Guid id) : AggregateRoot(id)
{
    #region Info

    private readonly string _title = null!;
    private string _description = null!;

    public required string Title
    {
        get => _title;
        init
        {
            if (string.IsNullOrEmpty(value) || value.Length > 200) throw new NameLengthException();
            _title = value.GetUpper();
        }
    }

    public required string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 1500) throw new DescriptionLengthException();
            _description = value.GetUpper();
        }
    }

    public required FilmType Type { get; init; }

    public required int Year { get; init; }
    public required Uri PosterUrl { get; set; }

    #endregion

    #region Cdn

    private readonly List<Cdn> _cdnList = [];

    public required IReadOnlyCollection<Cdn> CdnList
    {
        get => _cdnList;
        init
        {
            if (value.Count == 0) throw new EmptyCdnsCollectionException();
            if (_cdnList.GroupBy(x => x.Name).Any(x => x.Count() > 1)) throw new DuplicateCdnException();
            _cdnList = value.ToList();
        }
    }

    public void AddOrChangeCdn(Cdn cdn)
    {
        _cdnList.RemoveAll(x => x.Name == cdn.Name);
        _cdnList.Add(cdn);
    }

    #endregion
}