using Films.Domain.Extensions;

namespace Films.Domain.Films.ValueObjects;

/// <summary>
/// Класс, представляющий информацию об актере.
/// </summary>
public class Actor(string name, string? description)
{
    /// <summary>
    /// Имя актера.
    /// </summary>
    public string Name { get; } = name.GetUpper();

    /// <summary>
    /// Описание актера.
    /// </summary>
    public string? Description { get; } = description;
}