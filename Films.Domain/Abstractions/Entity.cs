namespace Films.Domain.Abstractions;

public abstract class Entity(int id)
{
    public int Id { get; } = id;
}