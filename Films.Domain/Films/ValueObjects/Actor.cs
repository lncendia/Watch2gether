namespace Films.Domain.Films.ValueObjects;

public class Actor
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}