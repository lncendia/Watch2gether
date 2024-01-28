namespace Films.Infrastructure.Web.Models.FilmManagement;

public class ActorViewModel
{
    public ActorViewModel(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string? Description { get; }
}