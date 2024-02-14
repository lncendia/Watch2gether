using Room.Domain.Abstractions;
using Room.Domain.Users.ValueObjects;

namespace Room.Domain.Users.Entities;

public class User(Guid id) : AggregateRoot(id)
{
    public required string UserName { get; set; }

    public required Uri PhotoUrl { get; set; }

    public Allows Allows { get; set; } = new()
    {
        Beep = false,
        Scream = false,
        Change = false
    };
}