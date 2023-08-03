namespace Overoom.Domain.Users.ValueObjects;

public class Allows
{
    internal Allows(bool beep, bool scream, bool change)
    {
        Beep = beep;
        Scream = scream;
        Change = change;
    }

    public bool Beep { get; }
    public bool Scream { get; }
    public bool Change { get; }
}