using Films.Domain.Extensions;

namespace Films.Domain.Films.ValueObjects;

public class Actor(string name, string? description)
{
    public string Name { get; } = name.GetUpper();
    public string? Description { get; } = description;
}