namespace Watch2gether.Domain.Films.ValueObject;

public class FilmData
{
    public FilmData(double rating, string name, string? description, string? shortDescription,
        IEnumerable<string> genres, int year, IEnumerable<string> countries,
        IEnumerable<(string name, string description)> actors, IEnumerable<string> screenwriters,
        IEnumerable<string> directors, int? countSeasons, int? countEpisodes)
    {
        Rating = rating;
        Name = name;
        Description = description;
        Year = year;
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        ShortDescription = shortDescription;

        _genres = genres.ToList();
        _countries = countries.ToList();
        _actors = actors.Select(x => new ActorData(x.name, x.description)).ToList();
        _directors = directors.ToList();
        _screenwriters = screenwriters.ToList();
    }

    public string Name { get; }
    public string? Description { get; }
    public string? ShortDescription { get; }
    public int Year { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    private readonly double _rating;

    public double Rating
    {
        get => _rating;
        private init
        {
            if (value is < 0 or > 10)
                throw new ArgumentException("Rating must be between 0 and 10");
            _rating = value;
        }
    }

    private readonly List<string> _countries;
    private readonly List<string> _directors;
    private readonly List<string> _screenwriters;
    private readonly List<ActorData> _actors;
    private readonly List<string> _genres;

    public List<string> Genres => _genres.ToList();
    public List<string> Countries => _countries.ToList();
    public List<string> Directors => _directors.ToList();
    public List<ActorData> Actors => _actors.ToList();
    public List<string> Screenwriters => _screenwriters.ToList();
}