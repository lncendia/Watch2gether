using Room.Domain.Abstractions;
using Room.Domain.Users.ValueObjects;

namespace Room.Domain.Users.Entities;

public class User : AggregateRoot
{
    public required string UserName { get; set; }

    public required Uri PhotoUrl { get; set; }

    public Allows Allows { get; private set; } = new()
    {
        Beep = false,
        Scream = false,
        Change = false
    };
    

    public void UpdateAllows(bool beep, bool scream, bool change)
    {
        Allows = new Allows
        {
            Beep = beep,
            Scream = scream,
            Change = change
        };
    }
}